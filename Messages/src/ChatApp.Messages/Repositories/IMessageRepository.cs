﻿using ChatApp.Messages.Controllers.Dtos;
using ChatApp.Messages.Entities;
using System.Collections.Concurrent;

namespace ChatApp.Messages.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessages(Guid conversationId, int? pageIndex, CancellationToken cancellationToken = default);
    Task SaveAndDequeueMessagesAsync(ConcurrentQueue<MessageDtoRedis> messages);
}
