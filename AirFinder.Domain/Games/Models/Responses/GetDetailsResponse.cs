using AirFinder.Domain.Common;
using AirFinder.Domain.Games.Models.Dtos;

namespace AirFinder.Domain.Games.Models.Responses
{
    public class GetDetailsResponse : BaseResponse
    {
        public GameDto Game { get; set; } = new GameDto();
    }
}
