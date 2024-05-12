using System.Data.Entity;
using Messenger.Models;

namespace Messenger.Services;

public class ChatContext : DbContext
{
    public DbSet<Chat> Chats { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
            .ToTable("Chats")
            .HasKey(x => x.ChatId);

        modelBuilder.Entity<Chat>()
            //Тут праймари кей и not null.
            .HasMany(x => x.FirstUserId)
            .IsRequired();
    }
}