using AirFinder.Domain.People;
using AirFinder.Domain.Users.Enums;

namespace AirFinder.Domain.Users.Models.Requests
{
    public class UserAdminRequest : UserRequest
    {
        public UserRole Role { get; set; } = UserRole.Default;
    }
}
