namespace ChatApp.RealTimeCommunication.Stores;

public interface IUserConnectionsStore
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string userId, string connectionId);
    IEnumerable<string> GetConnections(string userId);
}
