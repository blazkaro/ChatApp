using ChatApp.Messages.Controllers.Dtos;
using ChatApp.Messages.Entities;
using System.Collections.Concurrent;

namespace ChatApp.Messages.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetLastMessagesUpToDateAsync(Guid conversationId, DateTime? upTo, CancellationToken cancellationToken = default);
    Task SaveAndDequeueMessagesAsync(ConcurrentQueue<MessageDtoRedis> messages);
}
