namespace AirFinder.Application.Users.Models.Request
{
    public class VerifyTokenRequest
    {
        public string Email { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;
    }
}
