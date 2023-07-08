using AirFinder.Domain.Games.Models.Enums;

namespace AirFinder.Domain.Games.Models.Requests
{
    public class ListGamesRequest
    {
        public int PageIndex { get; set; } = 0;
        public int ItemsPerPage { get; set; } = 15;
        public GameStatus GameStatus { get; set; } = GameStatus.All;

    }
}
