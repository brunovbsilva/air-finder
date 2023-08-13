using AirFinder.Domain.Battlegrounds.Models.Requests;
using AirFinder.Domain.Battlegrounds.Models.Responses;
using AirFinder.Domain.Common;

namespace AirFinder.Application.Battlegrounds.Services
{
    public interface IBattlegroundService
    {
        Task<GetBattlegroundsResponse?> GetBattlegrounds(Guid id);
        Task<BaseResponse?> CreateBattleground(Guid id, CreateBattlegroundRequest request);
        Task<BaseResponse?> DeleteBattleground(Guid id);
        Task<BaseResponse?> UpdateBattleground(Guid id, UpdateBattlegroundRequest request);
    }
}
