using AirFinder.Domain.Common;
using AirFinder.Domain.GameLogs.Enums;
using AirFinder.Domain.Games;
using AirFinder.Domain.Users;

namespace AirFinder.Domain.GameLogs
{
    public class GameLog : BaseModel
    {
        public GameLog(Guid gameId, Guid fromUserId, long date, string description)
        {
            GameId = gameId;
            UserId = fromUserId;
            CreationDate = date;
            LastUpdateDate = null;
            LastUpdateUserId = null;
            Status = GameLogStatus.Joined;
            Description = description;
            Game = null;
            User = null;
            LastUpdateUser = null;
        }
        public GameLog() { }
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public long CreationDate { get; set; } = DateTime.Now.Ticks;
        public long? LastUpdateDate { get; set; } = null;
        public Guid? LastUpdateUserId { get; set; } = null;
        public GameLogStatus Status { get; set; } = GameLogStatus.None;
        public string Description { get; set; } = String.Empty;
        public virtual Game? Game { get; set; } = null;
        public virtual User? User { get; set; } = null;
        public virtual User? LastUpdateUser { get; set; } = null;
    }
}
