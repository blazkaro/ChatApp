
using ChatApp.RealTimeCommunication.Stores;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using System.Text.Json;

namespace ChatApp.RealTimeCommunication.Services.Background;

public class RedisConversationEventsHandler : BackgroundService
{
    private readonly IHubContext<CommunicationHub> _hub;
    private readonly ISubscriber _sub;
    private readonly IUserConnectionsStore _userConnectionsStore;
    private readonly IUserConversationsStore _userConversationsStore;

    public RedisConversationEventsHandler(IHubContext<CommunicationHub> hub,
        IConnectionMultiplexer connectionMultiplexer,
        IUserConnectionsStore userConnectionsStore,
        IUserConversationsStore userConversationsStore)
    {
        _hub = hub;
        _sub = connectionMultiplexer.GetSubscriber();
        _userConnectionsStore = userConnectionsStore;
        _userConversationsStore = userConversationsStore;
    }

    private async Task SubscribeAllAsync()
    {
        await _sub.SubscribeAsync(RedisChannel.Literal("conversations:invitations:accepted"),
            HandleConversationJoin);

        await _sub.SubscribeAsync(RedisChannel.Literal("conversations:created"),
            HandleConversationJoin);
    }

    private class ConversationJoinedEvent
    {
        public string ConversationId { get; set; }
        public string UserId { get; set; }
    }

    private async void HandleConversationJoin(RedisChannel channel, RedisValue value)
    {
        try
        {
            var ev = JsonSerializer.Deserialize<ConversationJoinedEvent>(value, JsonSerializerOptions.Web);

            _userConversationsStore.AddConversation(ev.UserId, ev.ConversationId);

            var connections = _userConnectionsStore.GetConnections(ev.UserId);
            var tasks = new List<Task>(connections.Count());
            foreach (var conn in connections)
            {
                tasks.Add(_hub.Groups.AddToGroupAsync(conn, ev.ConversationId));
            }

            await Task.WhenAll(tasks);

            // TODO
            // It's going to be called from every RTC service instance, but keep it here for simplicity
            await _hub.Clients.Group(ev.ConversationId.ToString())
                .SendAsync("OnConversationJoin", ev.ConversationId, ev.UserId);
        }
        catch
        {
            return;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SubscribeAllAsync();
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
