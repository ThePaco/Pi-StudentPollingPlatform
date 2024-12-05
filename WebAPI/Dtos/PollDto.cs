using DAL.Models;
using System.Text.Json.Serialization;

namespace WebAPI.Dtos
{
    public class PollDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Tekst { get; set; } = null!;

    }
}
