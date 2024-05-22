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

    [HttpPost("register")]
    public async Task Register([FromBody] RegisterUserDTO dto)
    {
        if (dto.Login == null || dto.Password == null || dto.Username == null)
            throw new ApiException("LOL", HttpStatusCode.BadRequest);
        await authorizationService.Register(dto.Login, dto.Password, dto.Username, dto.ImageBase64).ConfigureAwait(false);
    }


    [HttpPost("signIn")]
    public async Task<Guid> SignIn([FromBody] SignInDTO dto)
    {
        if (dto.Login == null || dto.Password == null)
            throw new ApiException("LOL", HttpStatusCode.BadRequest);
        var sessionState = await authorizationService.SignIn(dto.Login, dto.Password).ConfigureAwait(false);
        Response.Cookies.Append("auth.sid", sessionState.AuthSid.ToString("N"), new CookieOptions()
        {
            Expires = sessionState.ExpiresAt,
        });
        Response.Cookies.Append("userId", sessionState.UserId.ToString("N"), new CookieOptions()
        {
            Expires = sessionState.ExpiresAt,
        });
        return sessionState.UserId;
    }

    [HttpPost("logout")]
    public async Task Logout()
    {
        await authorizationService.Logout().ConfigureAwait(false);
        Response.Cookies.Delete("auth.sid");
        Response.Cookies.Delete("userId");
    }
    
    [HttpGet("getState")]
    public async Task<Guid> GetState()
    {
        var state = await authorizationService.GetState().ConfigureAwait(false);
        return state.UserId;
    }
}


public class RegisterUserDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string ImageBase64 { get; set; }
}

public class SignInDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
}

