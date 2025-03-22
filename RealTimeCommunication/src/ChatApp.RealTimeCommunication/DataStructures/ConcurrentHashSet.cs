using System.Collections.Concurrent;

namespace ChatApp.RealTimeCommunication.DataStructures;

public class ConcurrentHashSet<TKey>
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, byte> _concurrentDictionary = new();

    public bool TryAdd(TKey key) => _concurrentDictionary.TryAdd(key, 0);
    public bool TryRemove(TKey key) => _concurrentDictionary.TryRemove(key, out _);
    public bool ContainsKey(TKey key) => _concurrentDictionary.ContainsKey(key);
}
