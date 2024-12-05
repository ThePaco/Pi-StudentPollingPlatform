using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
