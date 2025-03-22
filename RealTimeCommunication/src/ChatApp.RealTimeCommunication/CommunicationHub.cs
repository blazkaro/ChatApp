using ChatApp.RealTimeCommunication.Dtos;
using ChatApp.RealTimeCommunication.Extensions;
using ChatApp.RealTimeCommunication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.RealTimeCommunication;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommunicationHub : Hub
{
    private readonly IConversationsService _conversationsService;
    private readonly IAccessTokenService _accessTokenService;

    public CommunicationHub(IConversationsService conversationsService,
        IAccessTokenService accessTokenService)
    {
        _conversationsService = conversationsService;
        _accessTokenService = accessTokenService;
    }

    public override async Task OnConnectedAsync()
    {
        // These are never null because user is authenticated
        var callerId = Context.UserIdentifier!;
        var at = (await _accessTokenService.GetCurrentAccessTokenAsync(Context.ConnectionAborted))!;

        var userConversations = await _conversationsService.GetUserConversations(callerId, at, Context.ConnectionAborted);
        var addUserToGroupTasks = new List<Task>(userConversations.Count);
        foreach (var conv in userConversations)
        {
            addUserToGroupTasks.Add(Groups.AddToGroupAsync(Context.ConnectionId, conv.Id, Context.ConnectionAborted));
            Context.StoreConversation(conv.Id);
        }

        await Task.WhenAll(addUserToGroupTasks);
    }

    public async Task SendMessageAsync(SendMessageDto dto)
    {
        if (string.IsNullOrEmpty(dto.Content) || !Context.HasConversation(dto.ConversationId))
            return;

        var callerId = Context.UserIdentifier!;
        var receiveMessageDto = new ReceiveMessageDto(dto.Content, callerId, dto.ConversationId, DateTime.UtcNow);

        await Clients.Groups(dto.ConversationId).SendAsync(CommunicationHubMethods.ReceiveMessageAsync,
            receiveMessageDto,
            cancellationToken: Context.ConnectionAborted);
    }
}
