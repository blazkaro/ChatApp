namespace ChatApp.Messages.Entities;

public record Message(Guid Id, string Content, string SenderId, Guid ConversationId, DateTime SentAt)
{
}
