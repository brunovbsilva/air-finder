using AirFinder.Domain.Common;
using AirFinder.Domain.Games;
using AirFinder.Domain.Users;

namespace AirFinder.Domain.GameLogs
{
    public class GameLog : BaseModel
    {
        public GameLog(Guid gameId, Guid fromUserId)
        {
            GameId = gameId;
            UserId = fromUserId;
            JoinDate = DateTime.Now.Ticks;
            PaymentDate = null;
            Game = null;
            User = null;
        }
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public long JoinDate { get; set; } = 0;
        public long? PaymentDate { get; set; } = null;
        public long? ValidateDate { get; set; } = null;
        public virtual Game? Game { get; set; } = null;
        public virtual User? User { get; set; } = null;

    }
}
