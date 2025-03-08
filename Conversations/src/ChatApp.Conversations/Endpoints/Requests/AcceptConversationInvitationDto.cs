using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Conversations.Endpoints.Requests;

public record AcceptConversationInvitationDto([FromRoute] Guid InvitationId)
{
}
