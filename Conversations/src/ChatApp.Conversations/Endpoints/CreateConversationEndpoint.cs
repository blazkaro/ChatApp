using ChatApp.Conversations.Db;
using ChatApp.Conversations.Db.Entities;
using ChatApp.Conversations.Endpoints.Requests;
using ChatApp.Conversations.Endpoints.Responses;
using ChatApp.Conversations.Extensions;
using System.Security.Claims;

namespace ChatApp.Conversations.Endpoints;

public static class CreateConversationEndpoint
{
    public static async Task<IResult> CreateAsync(CreateConversationDto dto, ClaimsPrincipal user, ConversationsDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var conversation = new Conversation
        {
            Name = dto.Name,
            Members =
            [
                new ConversationMember
                {
                    UserId = user.GetUserId(),
                    IsAdmin = true
                }
            ]
        };

        dbContext.Conversations.Add(conversation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Created(conversation.Id.ToString(), new ConversationCreatedDto(conversation.Id));
    }
}
