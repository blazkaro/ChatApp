using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Conversations.Controllers.Dtos.Requests;

public record CreateConversationInvitationDto([Required(AllowEmptyStrings = false)] string UserToInvite)
{
}
