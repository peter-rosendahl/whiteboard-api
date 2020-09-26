using System.ComponentModel.DataAnnotations;

namespace Whiteboard.API.DTO
{
    public class UserRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
        public string Password { get; set; }
        public string Email { get; set; }
    }
}