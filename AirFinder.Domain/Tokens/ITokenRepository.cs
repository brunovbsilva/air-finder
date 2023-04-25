namespace AirFinder.Domain.Tokens
{
    public interface ITokenRepository : IBaseRepository<TokenControl>
    {
        Task<TokenControl> GetByToken(string token);
    }
}
