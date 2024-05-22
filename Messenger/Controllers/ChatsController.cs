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

    //ПОЧЕМУ ГЕТ НЕ ПОДДЕРЖИВАЕТ BODY
    [HttpPost("get-users")]
    public async Task<UserModel[]> GetUsers([FromBody] GetUsersDto dto)
    {
        return await chattingService.GetUsers(dto.UsersIds.Select(Guid.Parse).ToArray()).ConfigureAwait(false);
    }
    
    [HttpPost("create")]
    public async Task<Chat> CreateChat([FromBody] CreateChatDto dto)
    {
        return await chattingService.CreateChat(dto.ChatName, dto.ImageBase64).ConfigureAwait(false);
    }

    [HttpPost("join")]
    public async Task JoinChat([FromBody] JoinChatDto dto)
    {
        await chattingService.JoinChat(dto.ChatId).ConfigureAwait(false);
    }
    
    [HttpPost("send-message")]
    public async Task SendMessage([FromBody] SendMessageDto dto)
    {
        await chattingService.SendMessage(dto.ChatId, dto.Text).ConfigureAwait(false);
    }
    
    [HttpPost("get-messages")]
    public async Task<Message[]> GetMessages([FromBody] GetMessagesDto dto)
    {
        return await chattingService.GetMessages(dto.ChatId).ConfigureAwait(false);
    }
}

public record CreateChatDto(string ChatName, string ImageBase64);
public class GetUsersDto
{
    public string[] UsersIds { get; set; }
}

public record JoinChatDto(Guid ChatId);
public record SendMessageDto(Guid ChatId, string Text);
public record GetMessagesDto(Guid ChatId);