using DAL.Models;
using System.Text.Json.Serialization;

namespace WebAPI.Dtos
{
    public class RoleDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

    }
}
