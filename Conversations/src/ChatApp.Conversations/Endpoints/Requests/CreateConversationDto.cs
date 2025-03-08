using System.ComponentModel.DataAnnotations;

namespace ChatApp.Conversations.Endpoints.Requests;

public record CreateConversationDto([Required(AllowEmptyStrings = false, ErrorMessage = "Conversation name cannot be empty")] string Name)
{
}
