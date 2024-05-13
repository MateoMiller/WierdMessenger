using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Messenger.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly AuthSidProvider sidProvider;
    private readonly SessionStateProvider sessionStateProvider;

    public AuthorizationService(SessionStateProvider sessionStateProvider, AuthSidProvider sidProvider)
    {
        this.sessionStateProvider = sessionStateProvider;
        this.sidProvider = sidProvider;
    }

    public async Task Register(string login, string password)
    {
        await using var context = new AuthContext();
        try
        {
            var passHash = PasswordEncryptor.CalculateHash(password);
            await context.AuthModels.AddAsync(
                new AuthModel
                {
                    Login = login,
                    PasswordHash = passHash,
                    UserId = Guid.NewGuid()
                }).ConfigureAwait(false);
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
        await using var context = new AuthContext();
        var passHash = PasswordEncryptor.CalculateHash(password);
        var user = await context.AuthModels
            .FirstAsync(x => x.Login == login && x.PasswordHash == passHash)
            .ConfigureAwait(false);
        if (user == null)
            throw new ApiException("User with this login + password does not exist");

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
        await using var context = new AuthContext();

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
    Task Register(string login, string password);
    Task<SessionState> SignIn(string login, string password);
    Task Logout();
    Task<SessionState> GetState();
}