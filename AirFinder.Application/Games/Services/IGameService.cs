using AirFinder.Domain.Common;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.Games.Models.Responses;

namespace AirFinder.Application.Games.Services
{
    public interface IGameService
    {
        Task<BaseResponse> CreateGame(CreateGameRequest request, Guid userId);
        Task<ListGamesResponse> ListGames(ListGamesRequest request, Guid userId);
        Task<GetDetailsResponse> GetDetails(Guid id);
        Task<BaseResponse> UpdateGame(UpdateGameRequest request, Guid userId);
        Task<BaseResponse> DeleteGame(Guid id, Guid userId);
        Task<BaseResponse> JoinGame(JoinGameRequest request, Guid userId);
        Task<BaseResponse> LeaveGame(Guid gameId, Guid userId);
        Task<BaseResponse> PayGame(PayGameRequest request, Guid userId);
        Task<BaseResponse> ValidateGameJoin(ValidateGameJoinRequest request, Guid userId);
    }
}
