namespace ChatApp.Conversations.Controllers.Dtos.Responses;

public record GetConversationDto(Guid Id, string Name, Uri? AvatarUrl, bool IsAdmin)
{
}
