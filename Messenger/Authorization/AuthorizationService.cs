using System.Net;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Messenger.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly AuthSidProvider sidProvider;
    private readonly ILogger logger;
    private readonly SessionStateProvider sessionStateProvider;

    public AuthorizationService(SessionStateProvider sessionStateProvider, AuthSidProvider sidProvider, ILogger logger)
    {
        this.sessionStateProvider = sessionStateProvider;
        this.sidProvider = sidProvider;
        this.logger = logger;
    }

    public async Task Register(string login, string password, string username)
    {
        await using var context = new AuthContext(logger);
        try
        {
            var passHash = PasswordEncryptor.CalculateHash(password);
            var userId = Guid.NewGuid();
            await context.AuthModels.AddAsync(
                new AuthModel
                {
                    Login = login,
                    PasswordHash = passHash,
                    UserId = userId
                }).ConfigureAwait(false);

            var randomImage = "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";

            var user = new UserModel
            {
                UserId = userId,
                Username = username,
                ImageBase64 = randomImage
            };
            
            await context.UserModels.AddAsync(user).ConfigureAwait(false);
            
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException ex)
        {
            var innerException = (PostgresException) ex.InnerException;
            throw innerException.SqlState switch
            {
                PostgresErrorCodes.UniqueViolation => new ApiException("User with this login already exist", HttpStatusCode.Conflict),
                _ => new ApiException("WTF", HttpStatusCode.InternalServerError)
            };
        }
    }

    public async Task<SessionState> SignIn(string login, string password)
    {
        await using var context = new AuthContext(logger);
        var passHash = PasswordEncryptor.CalculateHash(password);
        var user = await context.AuthModels
            .FirstOrDefaultAsync(x => x.Login == login && x.PasswordHash == passHash)
            .ConfigureAwait(false);
        if (user == null)
            throw new ApiException("User with this login + password does not exist", HttpStatusCode.BadRequest);

        var cookies = new CookiesModel
        {
            AuthSid = Guid.NewGuid(),
            ExpiresAt = DateTime.Now.Add(TimeSpan.FromMinutes(10)).ToUniversalTime(),
            UserId = user.UserId
        };

        await context.CookiesModels.AddAsync(cookies).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);

        return new SessionState
        {
            AuthSid = cookies.AuthSid,
            ExpiresAt = cookies.ExpiresAt,
            UserId = user.UserId,
        };
    }

    public async Task Logout()
    {
        var sid = sidProvider.Get();
        await using var context = new AuthContext(logger);

        var cookie = context.CookiesModels.Remove(new CookiesModel()
        {
            AuthSid = sid
        });
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public Task<SessionState> GetState()
    {
        return sessionStateProvider.GetAsync();
    }
}

public interface IAuthorizationService
{
    Task Register(string login, string password, string username);
    Task<SessionState> SignIn(string login, string password);
    Task Logout();
    Task<SessionState> GetState();
}