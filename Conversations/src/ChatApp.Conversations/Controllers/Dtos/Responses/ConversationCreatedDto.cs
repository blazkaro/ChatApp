namespace ChatApp.Conversations.Controllers.Dtos.Responses;

public record ConversationCreatedDto(Guid Id, string Name, Uri? AvatarUrl)
{
}
