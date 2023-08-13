using AirFinder.Domain.Common;
using AirFinder.Domain.Games.Models.Dtos;

namespace AirFinder.Domain.Games.Models.Responses
{
    public class ListGamesResponse : BaseResponse
    {
        public List<GameCardDto> Games { get; set; } = new List<GameCardDto>();
        public int Length { get; set; } = 0;
        public int PageIndex { get; set; } = 0;
    }
}
