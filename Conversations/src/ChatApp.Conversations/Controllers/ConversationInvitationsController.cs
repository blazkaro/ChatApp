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
using System.Data;

namespace ChatApp.Conversations.Controllers;

[Route("")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ConversationInvitationsController : ControllerBase
{
    private readonly ConversationsDbContext _dbContext;
    private readonly IConversationEventsPublisher _conversationEvents;

    public ConversationInvitationsController(ConversationsDbContext dbContext,
        IConversationEventsPublisher conversationEvents)
    {
        _dbContext = dbContext;
        _conversationEvents = conversationEvents;
    }

    [HttpGet("invitations")]
    public async Task<IActionResult> GetUserInvitationsAsync(CancellationToken cancellationToken)
    {
        return Ok(
            await _dbContext.ConversationInvitations.AsNoTracking()
                .Where(invitation => invitation.InvitedUserId == User.GetUserId())
                .Select(invitation =>
                    new GetConversationInvitationDto(invitation.ConversationId,
                        invitation.Conversation.Name,
                        invitation.Conversation.AvatarUrl))
                .ToListAsync(cancellationToken)
            );
    }

    [HttpPost("{conversationId}/invitations")]
    public async Task<IActionResult> CreateInvitationAsync(Guid conversationId, [FromBody] CreateConversationInvitationDto dto,
        CancellationToken cancellationToken)
    {
        var isAdmin = await _dbContext.Conversations.AsNoTracking()
            .AnyAsync(conv => conv.Id == conversationId && conv.Members.Any(member => member.UserId == User.GetUserId()),
            cancellationToken);

        if (!isAdmin)
            return Forbid();

        var isTargetMemberOrInvitedAlready = await _dbContext.Conversations.AsNoTracking()
            .AnyAsync(conv => conv.Id == conversationId && conv.Members.Any(member => member.UserId == dto.UserToInvite),
            cancellationToken);

        var isTargetInvitedAlready = await _dbContext.ConversationInvitations.AsNoTracking()
            .AnyAsync(inv => inv.ConversationId == conversationId && inv.InvitedUserId == dto.UserToInvite,
            cancellationToken);

        if (isTargetMemberOrInvitedAlready || isTargetInvitedAlready)
            return NoContent();

        _dbContext.ConversationInvitations.Add(new ConversationInvitation
        {
            ConversationId = conversationId,
            InvitedUserId = dto.UserToInvite
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPost("{conversationId}/invitations/accept")]
    public async Task<IActionResult> AcceptInvitationAsync(Guid conversationId, CancellationToken cancellationToken)
    {
        var invitation = await _dbContext.ConversationInvitations
            .Include(p => p.Conversation)
            .ThenInclude(p => p.Members)
            .SingleOrDefaultAsync(p => p.ConversationId == conversationId && p.InvitedUserId == User.GetUserId(),
            cancellationToken);

        if (invitation is null)
            return NotFound();

        var conversationMember = new ConversationMember
        {
            Conversation = invitation.Conversation,
            UserId = User.GetUserId()
        };

        _dbContext.Add(conversationMember);
        _dbContext.ConversationInvitations.Remove(invitation);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _conversationEvents.InvitationAccepted(conversationId, User.GetUserId());

        return NoContent();
    }
}
