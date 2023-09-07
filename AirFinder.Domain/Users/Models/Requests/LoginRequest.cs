namespace AirFinder.Domain.Users.Models.Requests
{
    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
