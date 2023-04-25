using AirFinder.Domain.People.Enums;

namespace AirFinder.Application.Users.Models.Request
{
    public class UserRequest
    {
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public DateTime Birthday { get; set; }
        public string CPF { get; set; } = String.Empty;
        public Gender Gender { get; set; }
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
    }
}
