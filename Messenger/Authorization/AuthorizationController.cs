using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Authorization;

[Route("auth")]
public class AuthorizationController : ControllerBase
{
    [HttpGet("signIn")]
    public async Task SignIn(string login, string password)
    {
        throw new ApiException("LOL", HttpStatusCode.BadRequest);
    }
    
    
    [HttpGet("logout")]
    public async Task Logout()
    {
        
        throw new NullReferenceException("Your account does not exist");
    }
}