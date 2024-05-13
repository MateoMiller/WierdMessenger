using System.Net;

namespace Messenger.Authorization;

public class AuthSidProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuthSidProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid Get()
    {
        var sid = httpContextAccessor.HttpContext.Request.Cookies["auth.sid"];
        if (sid == null)
            throw new ApiException("No sid found in cookies", HttpStatusCode.Unauthorized);
        if (Guid.TryParseExact(sid, "N", out var sidAsGuid))
            return sidAsGuid;
        throw new ApiException("Invalid auth sid", HttpStatusCode.Unauthorized);
    }
}