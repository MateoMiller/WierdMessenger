using System.ComponentModel.DataAnnotations;

namespace Messenger.Models;

public class Message
{
    public Guid MessageId { get; set; }
    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
    
    public string MessageText { get; set; }
    public DateTime SendDate { get; set; }
}