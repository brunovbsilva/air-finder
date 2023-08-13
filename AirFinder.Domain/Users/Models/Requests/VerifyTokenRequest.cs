namespace AirFinder.Domain.Users.Models.Requests
{
    public class VerifyTokenRequest
    {
        public string Email { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;
    }
}
