 using AirFinder.Domain.Tokens;

namespace AirFinder.Application.Tests.Mocks
{
    public class TokenControlMocks
    {
        public static TokenControl Default()
        {
            return new TokenControl(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<long>(),
                It.IsAny<long>()
            )
            {
                Id = It.IsAny<Guid>(),
            };
        }
    }
}
