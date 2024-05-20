using Microsoft.EntityFrameworkCore;

namespace Messenger.Authorization;

public class SessionStateProvider
{
    private AuthSidProvider sidProvider;
    private readonly ILogger logger;

    public SessionStateProvider(AuthSidProvider sidProvider, ILogger logger)
    {
        this.sidProvider = sidProvider;
        this.logger = logger;
        //TODO cache
    }

    public async Task<SessionState> GetAsync()
    {
        var sessionId = sidProvider.Get();
        await using var context = new AuthContext(logger);
        var kek = await context.CookiesModels.FirstAsync(x => x.AuthSid == sessionId);

        logger.Debug($"found {sessionId} {kek.UserId}");
        return new SessionState
        {
            AuthSid = sessionId,
            UserId = kek.UserId
        };
    }
}