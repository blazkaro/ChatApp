namespace ChatApp.Conversations.Db.Entities;

public class Conversation
{
    public long Id { get; set; }
    public string Name { get; set; }
    public ICollection<ConversationMember> Members { get; set; }
}
