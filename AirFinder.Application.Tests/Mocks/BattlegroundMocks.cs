using AirFinder.Domain.Battlegrounds;

namespace AirFinder.Application.Tests.Mocks
{
    public class BattlegroundMocks
    {
        public static Battleground Default()
        {
            var user = UserMocks.Default();
            return new Battleground(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                user.Id
            )
            {
                Id = It.IsAny<Guid>(),
                Creator = user
            };
        }
    }
}
