using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Conversations.Endpoints.Requests;

public record CreateConversationInvitationDto([FromRoute] Guid ConversationId,
    [Required(AllowEmptyStrings = false)] string InvitedUserId)
{
}
