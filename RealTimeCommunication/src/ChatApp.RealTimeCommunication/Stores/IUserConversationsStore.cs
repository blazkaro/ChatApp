namespace ChatApp.RealTimeCommunication.Stores;

public interface IUserConversationsStore
{
    void AddConversation(string userId, string conversationId);
    void RemoveConversaation(string userId, string conversationId);
    bool Exists(string userId, string conversationId);
}
