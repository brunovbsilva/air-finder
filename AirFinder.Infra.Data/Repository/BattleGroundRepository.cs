using AirFinder.Domain.BattleGrounds;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Infra.Data.Repository
{
    public class BattleGroundRepository : BaseRepository<BattleGround>, IBattleGroundRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public BattleGroundRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
