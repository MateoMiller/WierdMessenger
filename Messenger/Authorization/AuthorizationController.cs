using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Authorization;

[Route("auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService authorizationService;

    public AuthorizationController(IAuthorizationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    [HttpGet("register")]
    public async Task Register(string login, string password, string username)
    {
        if (login == null || password == null || username == null)
            throw new ApiException("LOL", HttpStatusCode.BadRequest);
        await authorizationService.Register(login, password, username).ConfigureAwait(false);
    }

    [HttpGet("signIn")]
    public async Task<Guid> SignIn(string login, string password)
    {
        if (login == null || password == null)
            throw new ApiException("LOL", HttpStatusCode.BadRequest);
        var sessionState = await authorizationService.SignIn(login, password).ConfigureAwait(false);
        Response.Cookies.Append("auth.sid", sessionState.AuthSid.ToString("N"), new CookieOptions()
        {
            Expires = sessionState.ExpiresAt
        });
        return sessionState.UserId;
    }

    [HttpGet("logout")]
    public async Task Logout()
    {
        await authorizationService.Logout().ConfigureAwait(false);
        Response.Cookies.Delete("auth.sid");
    }
    
    [HttpGet("getState")]
    public async Task<Guid> GetState()
    {
        var state = await authorizationService.GetState().ConfigureAwait(false);
        return state.UserId;
    }
}