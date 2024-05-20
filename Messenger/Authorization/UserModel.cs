namespace Messenger.Authorization;

public class UserModel
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string ImageBase64 { get; set; }
}