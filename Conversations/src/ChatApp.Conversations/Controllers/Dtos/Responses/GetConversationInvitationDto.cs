namespace ChatApp.Conversations.Controllers.Dtos.Responses;

public record GetConversationInvitationDto(Guid ConversationId, Guid InvitationId)
{
}
