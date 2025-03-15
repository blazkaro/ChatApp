using System.Security.Authentication;
using System.Security.Claims;

namespace ChatApp.Messages.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal?.Identity?.Name
            ?? throw new AuthenticationException($"The {nameof(claimsPrincipal)} doesn't represent authenticated entity with ID assigned");
        
        return id;
    }
}
