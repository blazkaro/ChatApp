using ChatApp.Spa.Server.Dtos;

namespace ChatApp.Spa.Server.Services;

public interface IUserInfoService
{
    Task<IEnumerable<UserInfoDto>> GetUsersByIdAsync(CancellationToken cancellationToken = default, params IEnumerable<string> usersIds);
    Task<IEnumerable<UserInfoDto>> GetUsersByNamePrefixAsync(string namePrefix, CancellationToken cancellationToken = default);
}
