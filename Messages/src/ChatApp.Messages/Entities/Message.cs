namespace ChatApp.Messages.Entities;

public record Message
{
    public Message()
    {
    }

    public Message(Guid Id, string Content, string SenderId, Guid ConversationId, DateTime SentAt)
    {
        this.Id = Id;
        this.Content = Content;
        this.SenderId = SenderId;
        this.ConversationId = ConversationId;
        this.SentAt = SentAt;
    }

    public Guid Id { get; init; }
    public string Content { get; init; }
    public string SenderId { get; init; }
    public Guid ConversationId { get; init; }
    public DateTime SentAt { get; init; }
}
