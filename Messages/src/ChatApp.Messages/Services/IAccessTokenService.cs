namespace ChatApp.Messages.Services;

public interface IAccessTokenService
{
    Task<string?> GetCurrentAccessTokenAsync(CancellationToken cancellationToken = default);
}
