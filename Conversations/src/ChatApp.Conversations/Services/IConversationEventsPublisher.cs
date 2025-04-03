namespace ChatApp.Conversations.Services;

public interface IConversationEventsPublisher
{
    Task InvitationAccepted(Guid conversationId, string userId);
    Task ConversationCreated(Guid conversationId, string userId);
}
