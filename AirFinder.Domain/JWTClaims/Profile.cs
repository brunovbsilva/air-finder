using AirFinder.Domain.Users;

namespace AirFinder.Domain.JWTClaims
{
    public class Profile
    {
        public Profile() { }
        public Profile(User user)
        {
            UserId = user.Id;
            PersonId = user.IdPerson;
            Login = user.Login;
            Name = user.Person!.Name;
            Email = user.Person!.Email;
            ImageUrl = user.Person.ImageUrl ?? String.Empty;
        }
        public Guid UserId { get; set; }
        public Guid PersonId { get; set; }
        public string Login { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;
    }
}
