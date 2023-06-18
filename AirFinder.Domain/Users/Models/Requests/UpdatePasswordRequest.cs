namespace AirFinder.Domain.Users.Models.Requests
{
    public class UpdatePasswordRequest
    {
        public string CurrentPassword { get; set; } = String.Empty;
        public string NewPassword { get; set; } = String.Empty;
    }
}
