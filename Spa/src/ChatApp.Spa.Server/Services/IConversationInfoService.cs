namespace ChatApp.Spa.Server.Services;

public interface IConversationInfoService
{
    Task<IEnumerable<string>> GetConversationMembersAsync(string conversationId, CancellationToken cancellationToken = default);
}
