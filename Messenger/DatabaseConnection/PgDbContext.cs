using Microsoft.EntityFrameworkCore;

namespace Messenger.DatabaseConnection;

public class PgDbContext : DbContext
{
    private readonly ILogger logger;

    public PgDbContext(ILogger logger)
    {
        this.logger = logger;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options
            .UseNpgsql(Program.PgConnectionString)
            .LogTo(data => logger.Debug(data), LogLevel.Information);
}