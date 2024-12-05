using System.ComponentModel.DataAnnotations;

namespace WebAPI.Security
{
    public class UserRegisterRequest
    {
        [Required, StringLength(50, MinimumLength = 2)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
