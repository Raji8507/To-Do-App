using System.Text.Json.Serialization;

namespace TodoDemoApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        // Role: "Manager" or "Employee"
        public string Role { get; set; } = "Employee";

        // Navigation properties
        [JsonIgnore]
        public ICollection<ToDo> CreatedToDos { get; set; } = new List<ToDo>();
        [JsonIgnore]
        public ICollection<ToDo> AssignedToDos { get; set; } = new List<ToDo>();
    }
}
