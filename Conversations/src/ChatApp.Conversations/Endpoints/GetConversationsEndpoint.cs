﻿using ChatApp.Conversations.Db;
using ChatApp.Conversations.Endpoints.Responses;
using ChatApp.Conversations.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Conversations.Endpoints;

public static class GetConversationsEndpoint
{
    public static async Task<IResult> GetAsync(ClaimsPrincipal user, ConversationsDbContext dbContext, CancellationToken cancellationToken)
    {
        return Results.Ok(
            await dbContext.Conversations.AsNoTracking()
                .Where(p => p.Members.Any(p => p.UserId == user.GetUserId()))
                .Select(p => new GetConversationDto(p.Id, p.Name)).ToListAsync(cancellationToken)
            );
    }
}
