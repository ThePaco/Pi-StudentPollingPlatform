using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMLogin
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
