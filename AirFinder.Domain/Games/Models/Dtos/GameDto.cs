using AirFinder.Domain.BattleGrounds.Models.Dtos;
using AirFinder.Domain.Common;

namespace AirFinder.Domain.Games.Models.Dtos
{
    public class GameDto : BaseModel
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long Date { get; set; }
        public Guid IdCreator { get; set; }
        public BattleGroundDto? BattleGround { get; set; }

        public static implicit operator GameDto(Game game)
        {
            if (game == null) return new GameDto();
            return new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Date = game.MillisDate,
                IdCreator = game.IdCreator,
                BattleGround = (BattleGroundDto)game.BattleGroud!
            };
        }
    }
}
