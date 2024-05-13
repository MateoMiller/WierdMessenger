namespace Messenger.Authorization;

public class AuthModel
{
    public string Login;
    public byte[] PasswordHash;
    public Guid UserId;
}