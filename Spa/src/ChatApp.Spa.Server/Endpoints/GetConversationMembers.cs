using ChatApp.Spa.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Spa.Server.Endpoints;

public static class GetConversationMembers
{
    public static async Task<IResult> GetAsync([FromRoute] string conversationId,
        IConversationInfoService conversationInfoService,
        IUserInfoService userInfoService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        if (user.Identity?.IsAuthenticated != true)
            return Results.Unauthorized();

        if (string.IsNullOrEmpty(conversationId))
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    { nameof(conversationId), [$"The {nameof(conversationId)} is required"] }
                });

        var members = await conversationInfoService.GetConversationMembersAsync(conversationId, cancellationToken);
        var membersInfo = await userInfoService.GetUsersByIdAsync(cancellationToken, members);
        return Results.Ok(membersInfo);
    }
}
