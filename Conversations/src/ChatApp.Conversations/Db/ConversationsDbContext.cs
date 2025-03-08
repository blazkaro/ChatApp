using ChatApp.Conversations.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Conversations.Db;

public class ConversationsDbContext : DbContext
{
    public ConversationsDbContext(DbContextOptions<ConversationsDbContext> options) : base(options)
    {
    }

    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationInvitation> ConversationInvitations { get; set; }

    protected ConversationsDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversation>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<ConversationMember>()
            .HasKey(p => new { p.UserId, p.ConversationId });

        modelBuilder.Entity<ConversationInvitation>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<ConversationInvitation>()
            .HasIndex(p => p.InvitedUserId);

        modelBuilder.Entity<ConversationInvitation>()
            .HasIndex(p => p.ConversationId);
    }
}
