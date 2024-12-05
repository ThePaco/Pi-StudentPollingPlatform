using DAL.Models;
using System.Text.Json.Serialization;

namespace WebAPI.Dtos
{
    public class QuestionDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Tekst { get; set; } = null!;

        public int PollId { get; set; }


    }
}
