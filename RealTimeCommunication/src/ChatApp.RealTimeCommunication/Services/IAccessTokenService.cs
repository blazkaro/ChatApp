namespace ChatApp.RealTimeCommunication.Services;

public interface IAccessTokenService
{
    Task<string?> GetCurrentAccessTokenAsync(CancellationToken cancellationToken = default);
}
