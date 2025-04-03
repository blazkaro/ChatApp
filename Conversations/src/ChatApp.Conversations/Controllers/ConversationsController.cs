using ChatApp.Conversations.Controllers.Dtos.Requests;
using ChatApp.Conversations.Controllers.Dtos.Responses;
using ChatApp.Conversations.Db;
using ChatApp.Conversations.Db.Entities;
using ChatApp.Conversations.Extensions;
using ChatApp.Conversations.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Conversations.Controllers;

[Route("")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ConversationsController : ControllerBase
{
    private readonly ConversationsDbContext _dbContext;
    private readonly IConversationEventsPublisher _conversationsEvents;

    public ConversationsController(ConversationsDbContext dbContext,
        IConversationEventsPublisher conversationsEvents)
    {
        _dbContext = dbContext;
        _conversationsEvents = conversationsEvents;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserConversations(CancellationToken cancellationToken)
    {
        return Ok(
            await _dbContext.Conversations.AsNoTracking()
                .Where(p => p.Members.Any(p => p.UserId == User.GetUserId()))
                .Select(p =>
                    new GetConversationDto(
                        p.Id,
                        p.Name,
                        p.AvatarUrl,
                        p.Members.Any(member => member.UserId == User.GetUserId() && member.IsAdmin)))
                .ToListAsync(cancellationToken)
            );
    }

    [HttpPost]
    public async Task<IActionResult> CreateConversationAsync([FromBody] CreateConversationDto dto, CancellationToken cancellationToken)
    {
        var conversation = new Conversation
        {
            Name = dto.Name,
            AvatarUrl = dto.AvatarUrl,
            Members =
            [
                new ConversationMember
                {
                    UserId = User.GetUserId(),
                    IsAdmin = true
                }
            ]
        };

        _dbContext.Conversations.Add(conversation);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _conversationsEvents.ConversationCreated(conversation.Id, User.GetUserId());

        return Created(conversation.Id.ToString(), new ConversationCreatedDto(conversation.Id, conversation.Name, conversation.AvatarUrl));
    }

    [HttpGet("{conversationId}/members")]
    public async Task<IActionResult> GetConversationMembers(Guid conversationId, CancellationToken cancellationToken)
    {
        if (!await VerifyMembership(conversationId, cancellationToken))
            return Forbid();

        return Ok(
            await _dbContext.Conversations.Where(p => p.Id == conversationId)
                .SelectMany(p => p.Members.Select(member => member.UserId))
                .ToListAsync(cancellationToken));
    }

    [HttpGet("membership/{conversationId}")]
    public async Task<bool> VerifyMembership(Guid conversationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Conversations
            .AnyAsync(conv => conv.Id == conversationId
                && conv.Members.Any(member => member.UserId == User.GetUserId()),
            cancellationToken);
    }
}
