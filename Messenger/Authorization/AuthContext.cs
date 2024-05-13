using Microsoft.EntityFrameworkCore;

namespace Messenger.Authorization;

public class AuthContext : DbContext
{
    public DbSet<AuthModel> AuthModels { get; set; }
    public DbSet<CookiesModel> CookiesModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthModel>(entity =>
        {
            entity.ToTable("authInfo");

            entity.HasKey(e => e.Login);

            entity.Property(e => e.Login)
                .IsRequired();

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(16)
                .IsFixedLength();
            
            entity.HasIndex(e => e.UserId)
                .IsUnique();

            entity.Property(e => e.UserId)
                .IsRequired();
        });
        
        
        modelBuilder.Entity<CookiesModel>(entity =>
        {
            entity.ToTable("cookies");

            entity.HasKey(e => e.AuthSid);

            entity.Property(e => e.AuthSid)
                .IsRequired();

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.Property(e => e.ExpiresAt)
                .IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(@"Host=localhost;Database=MyTest1;Username=postgres;Password=123");
}