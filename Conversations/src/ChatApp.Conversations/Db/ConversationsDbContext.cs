using ChatApp.Conversations.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Conversations.DbContexts;

public class ConversationsDbContext : DbContext
{
    public ConversationsDbContext(DbContextOptions<ConversationsDbContext> options) : base(options)
    {
    }

    public DbSet<Conversation> Conversations { get; set; }

    protected ConversationsDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversation>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<ConversationMember>()
            .HasKey(p => new { p.UserId, p.ConversationId });
    }
}
