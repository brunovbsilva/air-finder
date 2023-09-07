using AirFinder.Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class TokenRepository : BaseRepository<TokenControl>, ITokenRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public TokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenControl?> GetByToken(string token)
        {
            var tbToken = _unitOfWork.Context.Set<TokenControl>().AsNoTracking();
            var nowTicks = DateTime.Now.Ticks;

            var query = (from p in tbToken
                         where (p.Token == token && !string.IsNullOrEmpty(token) && p.Valid == true && (p.ExpirationDate == null || p.ExpirationDate >= nowTicks))
                         select p);
            return await query.OrderBy(x => x.SentDate).LastOrDefaultAsync();
        }
    }
}
