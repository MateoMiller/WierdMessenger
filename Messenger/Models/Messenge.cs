using System.ComponentModel.DataAnnotations;

namespace Messenger.Models;

public class Message
{
    public Guid Id;
    public Guid ChatId;
    
    public string MessageText;
    public DateTime SendDate;
}