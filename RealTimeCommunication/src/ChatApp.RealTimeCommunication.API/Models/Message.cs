namespace ChatApp.RealTimeCommunication.Models;

public record Message(string Content, string SenderId, string ConversationId, DateTime SentAt)
{
}
