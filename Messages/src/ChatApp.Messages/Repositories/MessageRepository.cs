using ChatApp.Messages.Controllers.Dtos;
using ChatApp.Messages.Entities;
using Dapper;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Concurrent;

namespace ChatApp.Messages.Repositories;

public class MessageRepository : IMessageRepository
{
    private const int MESSAGES_LIMIT = 50;

    private readonly NpgsqlDataSource _dataSource;

    public MessageRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<IEnumerable<Message>> GetLastMessagesUpToDateAsync(Guid conversationId, DateTime? upTo,
        CancellationToken cancellationToken = default)
    {
        await using var conn = await _dataSource.OpenConnectionAsync(cancellationToken);

        var queryParams = new { conversationId, upTo = upTo ?? DateTime.UtcNow.AddMinutes(10), messagesLimit = MESSAGES_LIMIT };
        var cmd = new CommandDefinition(@"SELECT Content, SenderId, ConversationId, SentAt FROM Messages
                                        WHERE ConversationId = @conversationId AND SentAt <= @upTo
                                        ORDER BY SentAt DESC
                                        LIMIT @messagesLimit", queryParams, cancellationToken: cancellationToken);

        return await conn.QueryAsync<Message>(cmd);
    }

    public async Task SaveAndDequeueMessagesAsync(ConcurrentQueue<MessageDtoRedis> messages)
    {
        await using var conn = await _dataSource.OpenConnectionAsync();
        await using var writer = await conn.BeginBinaryImportAsync(
            "COPY Messages (Content, SenderId, ConversationId, SentAt) FROM STDIN (FORMAT BINARY)"
            );

        int curCount = messages.Count;
        for (int i = 0; i < curCount; ++i)
        {
            if (messages.TryDequeue(out var msg))
            {
                await writer.StartRowAsync();

                await writer.WriteAsync(msg.Content, NpgsqlDbType.Varchar);
                await writer.WriteAsync(msg.SenderId, NpgsqlDbType.Varchar);
                await writer.WriteAsync(msg.ConversationId, NpgsqlDbType.Uuid);
                await writer.WriteAsync(msg.SentAt, NpgsqlDbType.TimestampTz);
            }
        }

        await writer.CompleteAsync();
    }
}
