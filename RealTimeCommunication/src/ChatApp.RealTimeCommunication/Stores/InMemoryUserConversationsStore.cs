using ChatApp.RealTimeCommunication.DataStructures;
using System.Collections.Concurrent;

namespace ChatApp.RealTimeCommunication.Stores;

public class InMemoryUserConversationsStore : IUserConversationsStore
{
    private readonly ConcurrentDictionary<string, ConcurrentHashSet<string>> _userConversations = new();

    public void AddConversation(string userId, string conversationId)
    {
        var conversations = _userConversations.GetOrAdd(userId, new ConcurrentHashSet<string>());
        conversations.TryAdd(conversationId);
    }

    public bool Exists(string userId, string conversationId)
    {
        return _userConversations.TryGetValue(userId, out var conversations)
            && conversations.ContainsKey(conversationId);
    }

    public void RemoveConversaation(string userId, string conversationId)
    {
        if (_userConversations.TryGetValue(userId, out var conversations))
        {
            conversations.TryRemove(conversationId);
        }
    }
}
