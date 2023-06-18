namespace AirFinder.Domain.Games.Models.Requests
{
    public class ValidateGameJoinRequest
    {
        public Guid IdGameLog { get; set; }
        public Guid UserId { get; set; }
    }
}
