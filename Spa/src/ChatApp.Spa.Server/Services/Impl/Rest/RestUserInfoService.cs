using ChatApp.Spa.Server.Configuration;
using ChatApp.Spa.Server.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json.Serialization;

namespace ChatApp.Spa.Server.Services.Impl.Rest;

public class RestUserInfoService : IUserInfoService
{
    private readonly ApiConfig _apiConfig;
    private readonly HttpClient _httpClient;

    private class UserInfoResponse
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        public string Nickname { get; set; }
        public string Picture { get; set; }
    }

    public RestUserInfoService(IOptions<ApiConfig> apiConfig, HttpClient httpClient)
    {
        _apiConfig = apiConfig.Value;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<UserInfoDto>> GetUsersByIdAsync(CancellationToken cancellationToken = default,
        params IEnumerable<string> usersIds)
    {
        if (!usersIds.Any())
            return [];

        var usersParam = new StringBuilder();
        foreach (var userId in usersIds)
        {
            usersParam.Append($"\"{userId}\" or ");
        }

        var queryString = new QueryBuilder
        {
            { "q", $"user_id:({usersParam})" },
        };

        return await GetAsync(queryString, cancellationToken);
    }

    public async Task<IEnumerable<UserInfoDto>> GetUsersByNamePrefixAsync(string namePrefix, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(namePrefix))
            return [];

        var queryString = new QueryBuilder
        {
            { "q", $"nickname:{namePrefix}*" }
        };

        return await GetAsync(queryString, cancellationToken);
    }

    private async Task<IEnumerable<UserInfoDto>> GetAsync(QueryBuilder queryString, CancellationToken cancellationToken)
    {
        queryString.Add("fields", "user_id,nickname,picture");

        var path = $"{_apiConfig.Auth0Api.GetUsersPath}{queryString}"[1..];// skip "/" to make path relative
        var result = await _httpClient.GetAsync(path, cancellationToken);
        result.EnsureSuccessStatusCode();

        var usersInfo = await result.Content.ReadFromJsonAsync<IEnumerable<UserInfoResponse>>(cancellationToken);
        return usersInfo?.Select(p =>
            new UserInfoDto
            {
                Id = p.UserId,
                Nickname = p.Nickname,
                AvatarUrl = new Uri(p.Picture)
            }) ?? [];
    }
}
