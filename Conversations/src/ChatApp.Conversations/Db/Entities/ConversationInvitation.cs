namespace ChatApp.Conversations.Db.Entities;

public class ConversationInvitation
{
    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; }
    public string InvitedUserId { get; set; }
}
