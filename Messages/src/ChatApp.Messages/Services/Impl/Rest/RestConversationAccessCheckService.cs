using ChatApp.Messages.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ChatApp.Messages.Services.Impl.Rest;

public class RestConversationAccessCheckService : IConversationAccessCheckService
{
    private readonly HttpClient _httpClient;
    private readonly ApiConfig _apiConfig;

    public RestConversationAccessCheckService(HttpClient httpClient, IOptions<ApiConfig> apiConfig)
    {
        _httpClient = httpClient;
        _apiConfig = apiConfig.Value;
    }

    public async Task<bool> HasAccesAsync(Guid conversationId, string accessToken, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"{_apiConfig.ConversationsApi.ConversationAccessCheckPath}/{conversationId}");
        req.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);

        var result = await _httpClient.SendAsync(req, cancellationToken);
        result.EnsureSuccessStatusCode();

        return await result.Content.ReadFromJsonAsync<bool>(cancellationToken);
    }
}
