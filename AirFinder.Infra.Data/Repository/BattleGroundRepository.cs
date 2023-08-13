using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Infra.Data.Repository
{
    public class BattlegroundRepository : BaseRepository<Battleground>, IBattlegroundRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public BattlegroundRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
