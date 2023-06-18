using AirFinder.Domain.BattleGrounds;
using AirFinder.Domain.Common;
using AirFinder.Domain.Games.Models.Enums;
using AirFinder.Domain.People;
using AirFinder.Domain.Users;

namespace AirFinder.Domain.Games
{
    public class Game : BaseModel
    {
        public Game(string name, string description, long millisDate, Guid idBattleGround, Guid idCreator)
        {
            Name = name;
            Description = description;
            MillisDate = millisDate;
            IdBattleGround = idBattleGround;
            IdCreator = idCreator;
            BattleGroud = null;
            Creator = null;
            Status = GameStatus.Created;
        }
        public Game() {}

        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long MillisDate { get; set; }
        public GameStatus Status { get; set; } = GameStatus.None;
        public Guid IdBattleGround { get; set; }
        public Guid IdCreator { get; set; }
        public virtual BattleGround? BattleGroud { get; set; }
        public virtual User? Creator { get; set; }
    }
}
