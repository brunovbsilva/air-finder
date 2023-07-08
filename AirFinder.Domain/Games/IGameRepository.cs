using AirFinder.Domain.GameLogs.Enums;
using AirFinder.Domain.Games.Models.Enums;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.Games.Models.Responses;

namespace AirFinder.Domain.Games
{
    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<ListGamesResponse> getGameList(ListGamesRequest request, Guid userId);
    }
}
