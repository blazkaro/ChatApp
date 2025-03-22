using ChatApp.RealTimeCommunication.Models;

namespace ChatApp.RealTimeCommunication.Services;

public interface IConversationsService
{
    Task<ICollection<Conversation>> GetUserConversations(string userId, string accessToken, CancellationToken cancellationToken = default);
}