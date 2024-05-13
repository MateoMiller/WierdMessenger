namespace Messenger.Authorization;

public class SessionState
{
    public Guid AuthSid { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}