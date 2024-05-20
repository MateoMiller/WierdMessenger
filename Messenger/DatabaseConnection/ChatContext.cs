using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DatabaseConnection;

public class ChatContext : PgDbContext
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserChat> UserChats { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Добавить constraint-ов
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("chats");

            entity.HasKey(x => x.ChatId);
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Base64Image);
        });
        
        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("messages");

            entity.HasKey(x => x.MessageId);
            entity.HasIndex(x => x.ChatId);
            
            entity.Property(x => x.MessageText).IsRequired();
            entity.Property(x => x.SendDate).IsRequired();
            entity.Property(x => x.SenderId).IsRequired();
        });

        modelBuilder.Entity<UserChat>(entity =>
        {
            entity.ToTable("userschats");

            entity.HasKey(x => new { x.UserId, x.ChatId });
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.ChatId);
        });

        base.OnModelCreating(modelBuilder);
    }

    public ChatContext(ILogger logger) : base(logger)
    {
    }
}