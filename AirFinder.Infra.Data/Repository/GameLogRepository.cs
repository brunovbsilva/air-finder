using AirFinder.Domain.GameLogs;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class GameLogRepository : BaseRepository<GameLog>, IGameLogRepository
    {
        public GameLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {}
    }
}
