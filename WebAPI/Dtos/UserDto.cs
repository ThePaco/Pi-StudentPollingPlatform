using DAL.Models;
using System.Text.Json.Serialization;

namespace WebAPI.Dtos
{
    public class UserDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PwdHash { get; set; } = null!;

        public string PwdSalt { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int RoleId { get; set; }

    }
}
