using DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class VMQuestion
    {
        public int Id { get; set; }

        [DisplayName("Question Text")]
        [Required(ErrorMessage = "Question text is required")]
        public string Tekst { get; set; } = null!;

        [DisplayName("Poll")]
        [Required(ErrorMessage = "Poll ID is required")]
        public int PollId { get; set; }

    }
}
