using System.ComponentModel.DataAnnotations.Schema;
using Messenger.Models;

namespace Messenger.Services;

public interface IUsersService
{
    Task<User[]> GetFittingUsers();
    Task<User[]> Friend();
}

public class UsersService
{
    
}