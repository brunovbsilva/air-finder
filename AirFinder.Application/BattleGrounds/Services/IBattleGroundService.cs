using AirFinder.Domain.Battlegrounds.Models.Requests;
using AirFinder.Domain.Battlegrounds.Models.Responses;
using AirFinder.Domain.Common;

namespace AirFinder.Application.Battlegrounds.Services
{
    public interface IBattlegroundService
    {
        Task<GetBattlegroundsResponse> GetBattlegrounds(Guid userId);
        Task<BaseResponse> CreateBattleground(Guid userId, CreateBattlegroundRequest request);
        Task<BaseResponse> DeleteBattleground(Guid userId, Guid id);
        Task<BaseResponse> UpdateBattleground(Guid userId, Guid id, UpdateBattlegroundRequest request);
    }
}
