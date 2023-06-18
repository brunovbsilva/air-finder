namespace AirFinder.Domain.Games.Models.Requests
{
    public class CreateGameRequest
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long Date { get; set; }
        public Guid IdBattleground { get; set; }
    }
}
