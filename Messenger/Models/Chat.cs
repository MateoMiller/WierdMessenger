namespace Messenger.Models;

public class Chat
{
    public Guid ChatId { get; set; }
    public string Name { get; set; }
    public string Base64Image { get; set; }
    public Message? LastSendMessage { get; set; }
}