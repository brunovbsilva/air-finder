namespace AirFinder.Domain.Users.Models.Requests
{
    public class ChangePasswordRequest
    {
        public string Email { get; set; } = String.Empty;
        public string NewPassword { get; set; } = String.Empty;
    }
}
