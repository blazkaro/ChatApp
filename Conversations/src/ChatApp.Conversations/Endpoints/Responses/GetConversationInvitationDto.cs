namespace ChatApp.Conversations.Endpoints.Responses;

public record GetConversationInvitationDto(Guid ConversationId, Guid InvitationId)
{
}
