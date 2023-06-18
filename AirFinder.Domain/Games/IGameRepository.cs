using AirFinder.Domain.GameLogs.Enums;
using AirFinder.Domain.Games.Models.Enums;
using AirFinder.Domain.Games.Models.Responses;

namespace AirFinder.Domain.Games
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<ListGamesResponse> getGameList(int page, int itensPerPage, List<GameLogStatus>? joinStatusList, List<GameStatus>? gameStatusList, long? from, long? upTo, Guid userId);
    }
}
