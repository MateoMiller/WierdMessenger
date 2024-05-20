using Messenger.Authorization;
using Messenger.Models;

namespace Messenger.Controllers;

public interface IChattingService
{
    public Task<Chat[]> GetAllChats();
    public Task<Chat> CreateChat(string chatName, string base64Image);
    public Task JoinChat(Guid chatId);
    public Task SendMessage(Guid chatId, string text);
    public Task<Message[]> GetMessages(Guid chatId);
    public Task<UserModel[]> GetUsers(Guid[] usersIds);
}