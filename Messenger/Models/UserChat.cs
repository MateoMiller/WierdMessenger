namespace Messenger.Models;

//Мб есть встроенный вариант маппинга? https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
public class UserChat
{
    public Guid UserId;
    public Guid ChatId;
}