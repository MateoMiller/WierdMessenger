using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("users")]

public class UsersController
{
    private readonly ILogger logger;

    public UsersController(ILogger<UsersController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("find-users")]
    public async Task FindUsers([FromBody] string nicknamePrefix)
    {
    }

    [HttpPost("add-friend")]
    public async Task AddFriends([FromBody] string userId)
    {
    }

    [HttpPost("delete-friend")]
    public async Task DeleleFriend([FromBody] string userId)
    {
    }
}