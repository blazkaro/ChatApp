using ChatApp.Conversations.Db;
using ChatApp.Conversations.Db.Entities;
using ChatApp.Conversations.Endpoints.Requests;
using ChatApp.Conversations.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Conversations.Endpoints;

public static class CreateConversationInvitationEndpoint
{
    public static async Task<IResult> CreateAsync(CreateConversationInvitationDto dto,
        ClaimsPrincipal user, ConversationsDbContext dbContext, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext.Conversations.AsNoTracking()
            .AnyAsync(conv => conv.Id == dto.ConversationId && conv.Members.Any(member => member.UserId == user.GetUserId()), cancellationToken);

        if (!isAdmin)
            return Results.Forbid();

        dbContext.ConversationInvitations.Add(new ConversationInvitation
        {
            ConversationId = dto.ConversationId,
            InvitedUserId = dto.InvitedUserId
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.Created();
    }
}
