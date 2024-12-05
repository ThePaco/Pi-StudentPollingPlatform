using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMPoll
    {
        public int Id { get; set; }

        [DisplayName("Poll Title")]
        [Required(ErrorMessage = "Poll title is required")]
        public string Title { get; set; } = null!;

        [DisplayName("Poll Text")]
        [Required(ErrorMessage = "Poll text is required")]
        public string Tekst { get; set; } = null!;
    }
}
    