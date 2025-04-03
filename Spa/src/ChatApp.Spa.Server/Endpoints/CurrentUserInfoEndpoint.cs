using ChatApp.Spa.Server.Dtos;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Spa.Server.Endpoints;

public static class CurrentUserInfoEndpoint
{
    public static async Task<IResult> GetAsync(HttpContext ctx, CancellationToken cancellationToken)
    {
        if (ctx.User.Identity?.IsAuthenticated != true)
            return Results.Unauthorized();

        var user = ctx.User;
        return Results.Ok(new UserInfoDto
        {
            Id = user.FindFirstValue(ClaimTypes.NameIdentifier),
            Nickname = user.FindFirstValue(JwtClaimTypes.NickName),
            AvatarUrl = new Uri(user.FindFirstValue(JwtClaimTypes.Picture))
        });
    }
}
