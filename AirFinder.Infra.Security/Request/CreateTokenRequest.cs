using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;

namespace AirFinder.Infra.Security.Request
{
    public class CreateTokenRequest
    {
        public CreateTokenRequest(User user)
        {
            Login = user.Login;
            UserId = user.Id;
            Name = user.Person!.Name;
            Scopes = new List<string> { user.Role == UserRole.Admnistrator ? "Adm_Role" : "User_Role" };
        }
        public string Login { get; set; } = String.Empty;
        public Guid UserId { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<string> Scopes { get; set; } = new List<string>();
    }
}
