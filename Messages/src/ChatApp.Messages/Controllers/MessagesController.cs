using ChatApp.Messages.Controllers.Dtos.Requests;
using ChatApp.Messages.Controllers.Dtos.Responses;
using ChatApp.Messages.Repositories;
using ChatApp.Messages.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Messages.Controllers;

[Route("")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MessagesController : ControllerBase
{
    private readonly IAccessTokenService _atService;
    private readonly IConversationAccessCheckService _conversationAccessCheckService;
    private readonly IMessageRepository _messageRepository;

    public MessagesController(IAccessTokenService atService,
        IConversationAccessCheckService conversationAccessCheckService,
        IMessageRepository messageRepository)
    {
        _atService = atService;
        _conversationAccessCheckService = conversationAccessCheckService;
        _messageRepository = messageRepository;
    }

    [HttpGet("{conversationId}")]
    public async Task<IActionResult> GetAsync(Guid conversationId, [FromQuery] GetMessagesReqDto dto, CancellationToken cancellationToken)
    {
        var at = (await _atService.GetCurrentAccessTokenAsync(cancellationToken))!; // not null because JWT auth is required
        var authorized = await _conversationAccessCheckService.HasAccesAsync(conversationId, at, cancellationToken);
        if (!authorized)
            return Forbid();

        return Ok(
            (await _messageRepository.GetLastMessagesUpToDateAsync(conversationId, dto.UpTo, cancellationToken))
                .Select(msg => new GetMessageResDto(msg.Id, msg.Content, msg.SenderId, msg.SentAt))
            );
    }
}
