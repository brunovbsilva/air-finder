using AirFinder.Domain.JWTClaims;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Security.Request
{
    [ExcludeFromCodeCoverage]
    public class CreateTokenRequest
    {
        public CreateTokenRequest(User user)
        {
            Profile = new Profile(user);
            Scopes = new List<string> { user.Role == UserRole.Admnistrator ? "Adm_Role" : "User_Role" };
        }
        public Profile Profile { get; set; }
        public List<string> Scopes { get; set; } = new List<string>();
    }
}
