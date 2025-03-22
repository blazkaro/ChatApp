using Microsoft.AspNetCore.Authentication;

namespace ChatApp.RealTimeCommunication.Services.Impl;

public class AccessTokenService : IAccessTokenService
{
    private readonly IHttpContextAccessor _ctxAccessor;

    public AccessTokenService(IHttpContextAccessor ctxAccessor)
    {
        _ctxAccessor = ctxAccessor;
    }

    public async Task<string?> GetCurrentAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_ctxAccessor.HttpContext is null)
            return null;

        return await _ctxAccessor.HttpContext.GetTokenAsync("access_token");
    }
}
