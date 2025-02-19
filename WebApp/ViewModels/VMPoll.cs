﻿using System.ComponentModel;
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

        [DisplayName("Poll Date")]
        [Required(ErrorMessage = "Poll date is required")]
        [DataType(DataType.Date)]
        public DateTime PollDate { get; set; }

        [DisplayName("Kolegij")]
        [Required(ErrorMessage = "Odaberite kolegij")]
        public int KolegijId { get; set; }

        [DisplayName("Studij")]
        [Required(ErrorMessage = "Odaberite studij")]
        public int StudijId { get; set; }

        [DisplayName("Godina Studija")]
        [Required(ErrorMessage = "Odaberite godinu studija")]
        public int GodinaId { get; set; }
    }
}