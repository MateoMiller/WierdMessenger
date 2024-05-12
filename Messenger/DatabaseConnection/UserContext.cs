using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Services;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasKey(x => x.Id);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer($"Server=5CG0121VBJ\\SQLEXPRESS;Database=MessTest;Trusted_Connection=True;");
}