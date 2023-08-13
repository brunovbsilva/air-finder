using AirFinder.Domain.BattleGrounds.Models.Requests;
using AirFinder.Domain.BattleGrounds.Models.Responses;
using AirFinder.Domain.Common;

namespace AirFinder.Application.BattleGrounds.Services
{
    public interface IBattleGroundService
    {
        Task<BaseResponse?> CreateBattleGround(Guid id, CreateBattleGroundRequest request);
        Task<GetBattleGroundResponse?> GetBattleGrounds(Guid id);
        Task<BaseResponse?> DeleteBattleGround(Guid id);
        Task<BaseResponse?> UpdateBattleGround(Guid id, UpdateBattleGroundRequest request);
    }
}
