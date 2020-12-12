using System.Threading.Tasks;
using Whiteboard.API.Models;

namespace Whiteboard.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
         Task<User> Update(User user);
         Task<User> GetUser(string username);
         bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}