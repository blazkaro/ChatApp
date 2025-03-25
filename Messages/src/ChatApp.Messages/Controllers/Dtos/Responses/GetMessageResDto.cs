namespace ChatApp.Messages.Controllers.Dtos.Responses;

public record GetMessageResDto(Guid Id, string Content, string SenderId, DateTime SentAt)
{
}
