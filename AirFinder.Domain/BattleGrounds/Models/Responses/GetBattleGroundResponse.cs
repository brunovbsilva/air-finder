using AirFinder.Domain.Battlegrounds.Models.Dtos;
using AirFinder.Domain.Common;

namespace AirFinder.Domain.Battlegrounds.Models.Responses
{
    public class GetBattlegroundsResponse : BaseResponse
    {
        public List<BattlegroundDto> Battlegrounds { get; set; } = new List<BattlegroundDto>();
    }
}
