using System.ComponentModel.DataAnnotations;

namespace Whiteboard.API.DTO
{
    public class UserRegisterDTO : UserLoginDTO
    {
        public string Email { get; set; }
    }
}