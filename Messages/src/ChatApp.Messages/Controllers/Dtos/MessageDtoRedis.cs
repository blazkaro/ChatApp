namespace ChatApp.Messages.Controllers.Dtos;

public record MessageDtoRedis(string Content, string SenderId, Guid ConversationId, DateTime SentAt)
{
}
