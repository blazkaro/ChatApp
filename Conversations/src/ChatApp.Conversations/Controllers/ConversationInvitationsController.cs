using ChatApp.Conversations.Controllers.Dtos.Requests;
using ChatApp.Conversations.Controllers.Dtos.Responses;
using ChatApp.Conversations.Db;
using ChatApp.Conversations.Db.Entities;
using ChatApp.Conversations.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Conversations.Controllers;

[Route("invitations")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ConversationInvitationsController : ControllerBase
{
    private readonly ConversationsDbContext _dbContext;

    public ConversationInvitationsController(ConversationsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserInvitationsAsync(CancellationToken cancellationToken)
    {
        return Ok(
            await _dbContext.ConversationInvitations.AsNoTracking()
                .Where(invitation => invitation.InvitedUserId == User.GetUserId())
                .Select(invitation => new GetConversationInvitationDto(invitation.ConversationId, invitation.Id))
                .ToListAsync(cancellationToken)
            );
    }

    [HttpPost("{conversationId}")]
    public async Task<IActionResult> CreateInvitationAsync(Guid conversationId, [FromBody] CreateConversationInvitationDto dto,
        CancellationToken cancellationToken)
    {
        var isAdmin = await _dbContext.Conversations.AsNoTracking()
            .AnyAsync(conv => conv.Id == conversationId && conv.Members.Any(member => member.UserId == User.GetUserId()), cancellationToken);

        if (!isAdmin)
            return Forbid();

        _dbContext.ConversationInvitations.Add(new ConversationInvitation
        {
            ConversationId = conversationId,
            InvitedUserId = dto.UserToInvite
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPost("{conversationId}/accept")]
    public async Task<IActionResult> AcceptInvitationAsync(Guid conversationId, CancellationToken cancellationToken)
    {
        var invitation = await _dbContext.ConversationInvitations
            .Include(p => p.Conversation)
            .SingleOrDefaultAsync(p => p.Id == conversationId && p.InvitedUserId == User.GetUserId(), cancellationToken);

        if (invitation is null)
            return NotFound();

        var conversationMember = new ConversationMember
        {
            Conversation = invitation.Conversation,
            UserId = User.GetUserId()
        };

        invitation.Conversation.Members.Add(conversationMember);
        _dbContext.ConversationInvitations.Remove(invitation);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
