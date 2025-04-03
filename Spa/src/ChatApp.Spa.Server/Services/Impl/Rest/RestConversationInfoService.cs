
using ChatApp.Spa.Server.Configuration;
using Microsoft.Extensions.Options;
using System.Net;

namespace ChatApp.Spa.Server.Services.Impl.Rest;

public class RestConversationInfoService : IConversationInfoService
{
    private readonly ApiConfig _apiConfig;
    private readonly HttpClient _httpClient;

    public RestConversationInfoService(IOptions<ApiConfig> apiConfig, HttpClient httpClient)
    {
        _apiConfig = apiConfig.Value;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<string>> GetConversationMembersAsync(string conversationId,
        CancellationToken cancellationToken = default)
    {
        var path = _apiConfig.ConversationsApi.GetMembersPath.Value!.Replace("@conversationId", conversationId);
        var result = await _httpClient.GetAsync(path, cancellationToken);
        if (result.StatusCode == HttpStatusCode.Forbidden)
            return [];

        result.EnsureSuccessStatusCode();
        return await result.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken)
            ?? [];
    }
}
