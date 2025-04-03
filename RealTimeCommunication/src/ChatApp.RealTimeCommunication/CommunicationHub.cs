using ChatApp.RealTimeCommunication.Dtos;
using ChatApp.RealTimeCommunication.Services;
using ChatApp.RealTimeCommunication.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.RealTimeCommunication;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommunicationHub : Hub
{
    private readonly IConversationsService _conversationsService;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IUserConnectionsStore _userConnectionsStore;
    private readonly IUserConversationsStore _userConversationsStore;

    public CommunicationHub(IConversationsService conversationsService,
        IAccessTokenService accessTokenService,
        IUserConnectionsStore userConnectionsStore,
        IUserConversationsStore userConversationsStore)
    {
        _conversationsService = conversationsService;
        _accessTokenService = accessTokenService;
        _userConnectionsStore = userConnectionsStore;
        _userConversationsStore = userConversationsStore;
    }

    public override async Task OnConnectedAsync()
    {
        // These are never null because user is authenticated
        var userId = Context.UserIdentifier!;
        var at = (await _accessTokenService.GetCurrentAccessTokenAsync(Context.ConnectionAborted))!;

        _userConnectionsStore.AddConnection(userId, Context.ConnectionId);

        var userConversations = await _conversationsService.GetUserConversations(userId, at, Context.ConnectionAborted);
        var addUserToGroupTasks = new List<Task>(userConversations.Count);
        foreach (var conv in userConversations)
        {
            addUserToGroupTasks.Add(Groups.AddToGroupAsync(Context.ConnectionId, conv.Id, Context.ConnectionAborted));
            _userConversationsStore.AddConversation(userId, conv.Id);
        }

        await Task.WhenAll(addUserToGroupTasks);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _userConnectionsStore.RemoveConnection(Context.UserIdentifier, Context.ConnectionId);
    }

    public async Task SendMessageAsync(SendMessageDto? dto)
    {
        var userId = Context.UserIdentifier!;
        if (string.IsNullOrEmpty(dto.Content) || !_userConversationsStore.Exists(userId, dto.ConversationId))
            return;

        var receiveMessageDto = new ReceiveMessageDto(dto.Content, userId, dto.ConversationId, DateTime.UtcNow);
        await Clients.Groups(dto.ConversationId).SendAsync(CommunicationHubMethods.ReceiveMessageAsync,
            receiveMessageDto,
            cancellationToken: Context.ConnectionAborted);
    }
}
