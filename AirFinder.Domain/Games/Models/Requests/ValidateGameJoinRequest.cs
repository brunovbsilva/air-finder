namespace AirFinder.Domain.Games.Models.Requests
{
    public class ValidateGameJoinRequest
    {
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
    }
}
