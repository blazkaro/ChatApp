using ChatApp.Conversations.Db;
using ChatApp.Conversations.Endpoints.Responses;
using ChatApp.Conversations.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Conversations.Endpoints;

public static class GetConversationInvitationsEndpoint
{
    public static async Task<IResult> GetAsync(ClaimsPrincipal user, ConversationsDbContext dbContext, CancellationToken cancellationToken)
    {
        return Results.Ok(
            await dbContext.ConversationInvitations.AsNoTracking()
                .Where(invitation => invitation.InvitedUserId == user.GetUserId())
                .Select(invitation => new GetConversationInvitationDto(invitation.ConversationId, invitation.Id))
                .ToListAsync(cancellationToken)
            );
    }
}
