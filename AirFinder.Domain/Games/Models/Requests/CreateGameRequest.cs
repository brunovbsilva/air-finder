namespace AirFinder.Domain.Games.Models.Requests
{
    public class CreateGameRequest
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long DateFrom { get; set; } = 0;
        public long DateUpTo { get; set; } = 0;
        public int? MaxPlayers { get; set; } = 0;
        public Guid IdBattleground { get; set; }
    }
}
