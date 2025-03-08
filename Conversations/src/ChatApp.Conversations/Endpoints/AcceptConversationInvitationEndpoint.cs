using ChatApp.Conversations.Db;
using ChatApp.Conversations.Db.Entities;
using ChatApp.Conversations.Endpoints.Requests;
using ChatApp.Conversations.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Conversations.Endpoints;

public static class AcceptConversationInvitationEndpoint
{
    public static async Task<IResult> AcceptAsync(AcceptConversationInvitationDto dto, ClaimsPrincipal user, ConversationsDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var invitation = await dbContext.ConversationInvitations
            .Include(p => p.Conversation)
            .SingleOrDefaultAsync(p => p.Id == dto.InvitationId && p.InvitedUserId == user.GetUserId(), cancellationToken);

        if (invitation is null)
            return Results.NotFound();

        var conversationMember = new ConversationMember
        {
            Conversation = invitation.Conversation,
            UserId = user.GetUserId()
        };

        invitation.Conversation.Members.Add(conversationMember);
        dbContext.ConversationInvitations.Remove(invitation);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }
}
