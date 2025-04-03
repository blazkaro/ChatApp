using StackExchange.Redis;
using System.Text.Json;

namespace ChatApp.Conversations.Services.Impl;

public class RedisConversationEventsPublisher : IConversationEventsPublisher
{
    private readonly IDatabase _redis;

    public RedisConversationEventsPublisher(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task ConversationCreated(Guid conversationId, string userId)
    {
        var value = JsonSerializer.Serialize(new { conversationId, userId });
        await _redis.PublishAsync(RedisChannel.Literal("conversations:created"), value);
    }

    public async Task InvitationAccepted(Guid conversationId, string userId)
    {
        var value = JsonSerializer.Serialize(new { conversationId, userId });
        await _redis.PublishAsync(RedisChannel.Literal("conversations:invitations:accepted"), value);
    }
}
