using Messenger.Authorization;
using Messenger.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("chats")]
public class ChatsController
{
    private readonly IChattingService chattingService;

    public ChatsController(IChattingService chattingService)
    {
        this.chattingService = chattingService;
    }

    [HttpGet("")]
    public async Task<Chat[]> GetAllChats()
    {
        return await chattingService.GetAllChats().ConfigureAwait(false);
    }

    [HttpGet("get-users")]
    public async Task<UserModel[]> GetUsers(Guid[] usersIds)
    {
        return await chattingService.GetUsers(usersIds).ConfigureAwait(false);
    }
    
    [HttpPost("create")]
    public async Task<Chat> CreateChat(string chatName, string imageBase64)
    {
        return await chattingService.CreateChat(chatName, imageBase64).ConfigureAwait(false);
    }

    [HttpPost("join")]
    public async Task JoinChat(Guid chatId)
    {
        await chattingService.JoinChat(chatId).ConfigureAwait(false);
    }
    
    [HttpPost("send-message")]
    public async Task SendMessage(Guid chatId, string text)
    {
        await chattingService.SendMessage(chatId, text).ConfigureAwait(false);
    }
    
    [HttpGet("get-messages")]
    public async Task<Message[]> GetMessages(Guid chatId)
    {
        return await chattingService.GetMessages(chatId).ConfigureAwait(false);
    }
}