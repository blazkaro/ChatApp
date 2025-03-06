namespace ChatApp.Conversations.Db.Entities;

public class ConversationMember
{
    public string UserId { get; set; }
    public long ConversationId { get; set; }
    public Conversation Conversation { get; set; }
    public bool IsAdmin { get; set; }
}
