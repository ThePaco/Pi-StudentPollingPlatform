using DAL.Models;
using System.Text.Json.Serialization;

namespace WebAPI.Dtos
{
    public class UserAnswerDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public int Answer { get; set; }

    }
}
