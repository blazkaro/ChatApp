namespace ChatApp.Messages.Controllers.Dtos.Responses;

public record GetMessageResDto(string Content, string SenderId, DateTime SentAt)
{
}
