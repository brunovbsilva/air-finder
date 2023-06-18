using AirFinder.Domain.GameLogs.Enums;
using AirFinder.Domain.Games.Models.Enums;

namespace AirFinder.Domain.Games.Models.Dtos
{
    public class GameCardDto
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Local { get; set; } = String.Empty;
        public long Date { get; set; }
        public string? ImageUrl { get; set; } = String.Empty;
        public bool Verified { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public GameStatus GameStatus { get; set; } = GameStatus.None;
        public GameLogStatus JoinStatus { get; set; } = GameLogStatus.None;
        public GameCardDto() { }
    }
}
