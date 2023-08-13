using AirFinder.Domain.BattleGrounds.Models.Dtos;
using AirFinder.Domain.Common;

namespace AirFinder.Domain.Games.Models.Dtos
{
    public class GameDto : BaseModel
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long DateFrom { get; set; } = 0;
        public long DateUpTo { get; set; } = 0;
        public int MaxPlayers { get; set; } = 0;
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
                DateFrom = game.MillisDateFrom,
                DateUpTo = game.MillisDateUpTo,
                MaxPlayers = game.MaxPlayers,
                IdCreator = game.IdCreator,
                BattleGround = (BattleGroundDto)game.BattleGroud!
            };
        }
    }
}
