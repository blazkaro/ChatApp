using System.ComponentModel.DataAnnotations;

namespace ChatApp.Conversations.Controllers.Dtos.Requests;

public record CreateConversationDto([Required(AllowEmptyStrings = false)] string Name)
{
}
