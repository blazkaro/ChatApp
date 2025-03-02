namespace ChatApp.RealTimeCommunication.Dtos;

public record ReceiveMessageDto(string Content, string SenderId, string ConversationId, DateTime SentAt)
{
}

