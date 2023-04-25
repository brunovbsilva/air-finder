namespace AirFinder.Application.Users.Models.Request
{
    public class VerifyTokenRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
