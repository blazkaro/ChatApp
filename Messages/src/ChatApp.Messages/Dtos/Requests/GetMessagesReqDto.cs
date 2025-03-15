using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Messages.Dtos.Requests;

public record GetMessagesReqDto([FromRoute] Guid ConversationId, DateTime? UpTo)
{
}
