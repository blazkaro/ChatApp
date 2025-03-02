using ChatApp.RealTimeCommunication.Dtos;
using ChatApp.RealTimeCommunication.Extensions;
using ChatApp.RealTimeCommunication.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.RealTimeCommunication;

public class CommunicationHub : Hub
{
    private readonly IConversationsService _conversationsService;

    public CommunicationHub(IConversationsService conversationsService)
    {
        _conversationsService = conversationsService;
    }

    public override async Task OnConnectedAsync()
    {
        var callerId = Context.UserIdentifier!;
        var userConversations = await _conversationsService.GetUserConversations(callerId, Context.ConnectionAborted);

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
