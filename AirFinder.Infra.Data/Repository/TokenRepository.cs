using AirFinder.Domain.Tokens;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Infra.Data.Repository
{
    public class TokenRepository : BaseRepository<TokenControl>, ITokenRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public TokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenControl> GetByToken(string token)
        {
            var tbToken = _unitOfWork.Context.Set<TokenControl>().AsNoTracking();

            var query = (from p in tbToken.DefaultIfEmpty()
                         where (p.Token == token && !string.IsNullOrEmpty(token) &&
                            p.Valid == true && (p.ExpirationDate == null || p.ExpirationDate >= DateTime.Now))
                         select p);
            return await query.OrderBy(x => x.Id).LastOrDefaultAsync();
        }
    }
}
