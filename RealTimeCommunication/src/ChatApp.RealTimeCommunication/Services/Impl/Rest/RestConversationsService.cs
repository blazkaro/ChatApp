using ChatApp.RealTimeCommunication.Configuration;
using ChatApp.RealTimeCommunication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ChatApp.RealTimeCommunication.Services.Impl.Rest;

public class RestConversationsService : IConversationsService
{
    private readonly HttpClient _httpClient;
    private readonly ApiConfig _apiConfig;

    public RestConversationsService(HttpClient httpClient, IOptions<ApiConfig> apiConfig)
    {
        _httpClient = httpClient;
        _apiConfig = apiConfig.Value;
    }

    public async Task<ICollection<Conversation>> GetUserConversations(string userId, string accessToken,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, _apiConfig.ConversationsApi.GetConversationsPath);
        req.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);

        var result = await _httpClient.SendAsync(req, cancellationToken);
        result.EnsureSuccessStatusCode();

        return await result.Content.ReadFromJsonAsync<ICollection<Conversation>>(cancellationToken)
            ?? Array.Empty<Conversation>();
    }
}
