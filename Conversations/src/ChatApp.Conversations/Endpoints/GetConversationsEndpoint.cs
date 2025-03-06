using ChatApp.Conversations.DbContexts;
using ChatApp.Conversations.Endpoints.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Conversations.Endpoints;

public static class GetConversationsEndpoint
{
    public static async Task<IResult> GetConversationsAsync(ClaimsPrincipal user, ConversationsDbContext dbContext, CancellationToken cancellationToken)
    {
        var userId = user.Identity!.Name;
        return Results.Ok(
            await dbContext.Conversations.AsNoTracking()
            .Where(p => p.Members.Any(p => p.UserId == userId))
            .Select(p => new ConversationDto(p.Id, p.Name)).ToListAsync(cancellationToken)
            );
    }
}
