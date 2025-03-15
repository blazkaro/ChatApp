namespace ChatApp.Messages.Dtos;

public record MessageDtoRedis(string Content, string SenderId, Guid ConversationId, DateTime SentAt)
{
}
