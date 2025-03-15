namespace ChatApp.Messages.Dtos.Responses;

public record GetMessageResDto(Guid Id, string Content, string SenderId, Guid ConversationId, DateTime SentAt)
{
}
