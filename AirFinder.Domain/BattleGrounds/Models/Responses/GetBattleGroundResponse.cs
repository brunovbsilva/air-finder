using AirFinder.Domain.BattleGrounds.Models.Dtos;
using AirFinder.Domain.Common;

namespace AirFinder.Domain.BattleGrounds.Models.Responses
{
    public class GetBattleGroundResponse : BaseResponse
    {
        public List<BattleGroundDto> Battlegrounds { get; set; }
    }
}
