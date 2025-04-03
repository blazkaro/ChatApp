using ChatApp.Spa.Server.Dtos;
using ChatApp.Spa.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Spa.Server.Endpoints;

public static class UserInfoEndpoint
{
    public static async Task<IResult> GetAsync([FromQuery] string? namePrefix, IUserInfoService userInfoService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(namePrefix) || namePrefix.Any(p => !char.IsLetterOrDigit(p)))
            return Results.Ok(Array.Empty<UserInfoDto>());

        return Results.Ok(await userInfoService.GetUsersByNamePrefixAsync(namePrefix, cancellationToken));
    }
}
