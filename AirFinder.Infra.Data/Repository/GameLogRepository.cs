using AirFinder.Domain.GameLogs;

namespace AirFinder.Infra.Data.Repository
{
    public class GameLogRepository : BaseRepository<GameLog>, IGameLogRepository
    {
        public GameLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {}
    }
}
