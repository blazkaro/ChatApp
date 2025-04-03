using ChatApp.Messages.Controllers.Dtos;
using ChatApp.Messages.Repositories;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ChatApp.Messages.Services.Background;

public class SaveMessagesBackgroundService : BackgroundService
{
    private readonly TimeSpan MESSAGES_SAVE_RATE = TimeSpan.FromMilliseconds(1000);
    private readonly JsonSerializerOptions _messageSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly ConcurrentQueue<MessageDtoRedis> MessagesToSave = new();

    private readonly ISubscriber _redisSub;
    private readonly IMessageRepository _messageRepository;

    public SaveMessagesBackgroundService(IConnectionMultiplexer connectionMultiplexer,
        IMessageRepository messageRepository)
    {
        _redisSub = connectionMultiplexer.GetSubscriber();
        _messageRepository = messageRepository;
    }

    private void HandleMessage(RedisChannel channel, RedisValue value)
    {
        if (channel.IsNullOrEmpty || value.IsNullOrEmpty)
            return;

        var msg = ReadMessage(value);
        if (!ValidateMessage(msg))
            return;

        MessagesToSave.Enqueue(msg);
    }

    /// <summary>
    /// Depends on internal SignalR Redis backplane implementation
    /// </summary>
    private MessageDtoRedis? ReadMessage(RedisValue value)
    {
        try
        {
            var str = value.ToString();

            var startIndex = str.IndexOf('{');
            var endIndex = str.LastIndexOf('}');

            var json = JsonNode.Parse(str.AsSpan(startIndex, endIndex - startIndex + 1).ToString())?.AsObject();
            if (json is null || !json.TryGetPropertyValue("arguments", out JsonNode? argsNode)
                || argsNode is not JsonArray arguments || arguments.Count == 0)
                return null;

            return JsonSerializer.Deserialize<MessageDtoRedis>(arguments[0], _messageSerializerOptions);
        }
        catch
        {
            return null;
        }
    }

    private static bool ValidateMessage([NotNullWhen(true)] MessageDtoRedis? msg)
    {
        if (msg is null || msg.ConversationId == default || string.IsNullOrEmpty(msg.Content) || msg.SentAt == default)
            return false;

        return true;
    }

    private async Task SubscribeToMessagesChannel()
    {
        // TODO
        // will duplicate messages on multiple instances, the channel prefix should be included
        // leave it here for now for simplicity
        await _redisSub.SubscribeAsync(RedisChannel.Pattern("*RealTimeCommunication.CommunicationHub:group:*"), HandleMessage);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SubscribeToMessagesChannel();

        using PeriodicTimer timer = new(MESSAGES_SAVE_RATE);
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await _messageRepository.SaveAndDequeueMessagesAsync(MessagesToSave);
        }
    }
}
