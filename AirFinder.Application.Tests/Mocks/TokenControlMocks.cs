using AirFinder.Domain.Tokens;

namespace AirFinder.Application.Tests.Mocks
{
    public class TokenControlMocks
    {
        public static TokenControl TokenControlDefault()
        {
            return new TokenControl()
            {
                IdUser = It.IsAny<Guid>(),
                Token = It.IsAny<string>(),
                Valid = It.IsAny<bool>(),
                SentDate = It.IsAny<long>(),
                ExpirationDate = It.IsAny<long>()
            };
        }
        public static IEnumerable<TokenControl> TokenControlDefaultEnumerable()
        {
            return new EnumerableQuery<TokenControl>( new List<TokenControl> { TokenControlDefault() } );
        }
        public static IEnumerable<TokenControl> TokenControlDefaultEmptyEnumerable()
        {
            return new EnumerableQuery<TokenControl>( new List<TokenControl>() );
        }
    }
}
