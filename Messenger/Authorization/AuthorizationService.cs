namespace Messenger.Authorization;

public class AuthorizationService : IAuthorizationService
{
    public Task SignIn(string login, string password)
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }
}

public interface IAuthorizationService
{
    Task SignIn(string login, string password);
    Task Logout();
}