using Messenger.Authorization;
using Messenger.DatabaseConnection;
using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Controllers;

public class ChattingService : IChattingService
{
    private readonly ILogger logger;
    private readonly SessionState sessionState;

    public ChattingService(ILogger logger, SessionState sessionState)
    {
        this.logger = logger;
        this.sessionState = sessionState;
    }

    public async Task<Chat[]> GetAllChats()
    {
        await using var chatsContext = new ChatContext(logger);

        var chats = await chatsContext
            .UserChats
            .Join(chatsContext.Chats, y => y.ChatId, inner => inner.ChatId, (chat, inner) => inner)
            .ToArrayAsync();

        await chatsContext.SaveChangesAsync().ConfigureAwait(false);
        
        logger.Debug($"Found {chats.Length} chats for user {sessionState.UserId}");

        return chats;
    }

    public async Task<Chat> CreateChat(string chatName, string base64Image)
    {
        await using var chatsContext = new ChatContext(logger);

        var chatId = Guid.NewGuid();
        var chat = await chatsContext
            .Chats
            .AddAsync(
                new Chat
                {
                    ChatId = chatId,
                    Name = chatName,
                    Base64Image = base64Image
                }
            ).ConfigureAwait(false);
        
        await chatsContext
            .UserChats
            .AddAsync(
                new UserChat
                {
                    ChatId = chatId,
                    UserId = sessionState.UserId
                }
            ).ConfigureAwait(false);

        await chatsContext.SaveChangesAsync().ConfigureAwait(false);
        
        logger.Debug($"Created chat {chat.Entity.ChatId} for user {sessionState.UserId}");

        return chat.Entity;
    }

    public async Task JoinChat(Guid chatId)
    {
        await using var chatsContext = new ChatContext(logger);

        var chat = await chatsContext
            .Chats
            .FirstOrDefaultAsync(x => x.ChatId == chatId)
            .ConfigureAwait(false);

        if (chat == null)
            throw new ApiException($"Chat with id {chatId} does not exist");

        var chat2 = await chatsContext
            .UserChats
            .AddAsync(
                new UserChat()
                {
                    ChatId = chatId,
                    UserId = sessionState.UserId
                })
            .ConfigureAwait(false);

        await chatsContext.SaveChangesAsync().ConfigureAwait(false);
        
        logger.Debug($"User {sessionState.UserId} just joined chat {chatId}");
    }

    public async Task SendMessage(Guid chatId, string text)
    {
        await using var chatsContext = new ChatContext(logger);

        var message = await chatsContext
            .Messages
            .AddAsync(
                new Message
                {
                    ChatId = chatId,
                    MessageId = Guid.NewGuid(),
                    MessageText = text,
                    SendDate = DateTime.Now.ToUniversalTime(),
                    SenderId = sessionState.UserId
                })
            .ConfigureAwait(false);

        await chatsContext.SaveChangesAsync().ConfigureAwait(false);
        
        logger.Debug($"Created message {message.Entity.MessageId}");
    }

    public async Task<Message[]> GetMessages(Guid chatId)
    {
        await using var chatsContext = new ChatContext(logger);

        var messages = await chatsContext
            .Messages
            .Where(x => x.ChatId == chatId)
            .OrderBy(x => x.SendDate)
            .ToArrayAsync()
            .ConfigureAwait(false);

        await chatsContext.SaveChangesAsync().ConfigureAwait(false);
        
        logger.Debug($"Found {messages.Length} messages in chat {chatId}");

        return messages;
    }

    public async Task<UserModel[]> GetUsers(Guid[] usersIds)
    {
        await using var authContext = new AuthContext(logger);

        var users = await authContext
            .UserModels
            .Where(x => usersIds.Contains(x.UserId))
            .ToArrayAsync()
            .ConfigureAwait(false);
        
        logger.Debug($"Found {users.Length} users");

        return users;
    }
}