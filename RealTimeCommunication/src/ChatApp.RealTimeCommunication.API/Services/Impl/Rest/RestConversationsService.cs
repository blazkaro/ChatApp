using ChatApp.RealTimeCommunication.Configuration;
using ChatApp.RealTimeCommunication.Models;
using Microsoft.Extensions.Options;

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

    public async Task<ICollection<Conversation>> GetUserConversations(string userId, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.GetAsync(_apiConfig.ConversationsApi.GetConversationsPath, cancellationToken);
        result.EnsureSuccessStatusCode();

        return await result.Content.ReadFromJsonAsync<ICollection<Conversation>>(cancellationToken)
            ?? Array.Empty<Conversation>();
    }
}
