namespace ChatApp.Messages.Services;

public interface IConversationAccessCheckService
{
    Task<bool> HasAccesAsync(Guid conversationId, string accessToken, CancellationToken cancellationToken = default);
}
