using Messenger.DatabaseConnection;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Authorization;

public class AuthContext : PgDbContext
{
    public DbSet<AuthModel> AuthModels { get; set; }
    public DbSet<UserModel> UserModels { get; set; }
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
        
        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.Username)
                .IsRequired();

            entity.Property(e => e.ImageBase64)
                .IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }

    public AuthContext(Serilog.ILogger logger) : base(logger)
    {
    }
}