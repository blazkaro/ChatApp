using ChatApp.RealTimeCommunication.DataStructures;
using System.Collections.Concurrent;

namespace ChatApp.RealTimeCommunication.Stores;

public class InMemoryUserConnectionStore : IUserConnectionsStore
{
    private readonly ConcurrentDictionary<string, ConcurrentHashSet<string>> _userConnections = new();

    public void AddConnection(string userId, string connectionId)
    {
        var connections = _userConnections.GetOrAdd(userId, new ConcurrentHashSet<string>());
        connections.TryAdd(connectionId);
    }

    public void RemoveConnection(string userId, string connectionId)
    {
        if (_userConnections.TryGetValue(userId, out var connections))
        {
            connections.TryRemove(connectionId);
        }
    }

    public IEnumerable<string> GetConnections(string userId)
    {
        if (_userConnections.TryGetValue(userId, out var connections))
        {
            foreach (var conn in connections)
            {
                yield return conn.Key;
            }
        }
    }
}
