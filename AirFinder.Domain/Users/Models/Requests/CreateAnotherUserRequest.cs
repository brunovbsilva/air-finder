using AirFinder.Domain.People;
using AirFinder.Domain.Users.Enums;

namespace AirFinder.Domain.Users.Models.Requests
{
    public class CreateAnotherUserRequest
    {
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public UserRoll Role { get; set; } = UserRoll.Default;
    }
}
