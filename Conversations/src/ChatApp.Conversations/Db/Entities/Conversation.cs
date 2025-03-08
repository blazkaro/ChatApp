namespace ChatApp.Conversations.Db.Entities;

public class Conversation
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<ConversationMember> Members { get; set; }
}
