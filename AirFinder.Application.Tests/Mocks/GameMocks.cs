using AirFinder.Domain.Games;

namespace AirFinder.Application.Tests.Mocks
{
    public class GameMocks
    {
        public static Game Default()
        {
            var battleground = BattlegroundMocks.Default();
            var user = UserMocks.Default();
            return new Game(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<int>(),
                battleground.Id,
                user.Id
            )
            {
                Id = It.IsAny<Guid>(),
                BattleGroud = battleground,
                Creator = user
            };
        }
        public static IEnumerable<Game> DefaultEnumerable()
        {
            return new EnumerableQuery<Game>(new List<Game> { Default() });
        }
        public static IEnumerable<Game> DefaultEmptyEnumerable()
        {
            return new EnumerableQuery<Game>(new List<Game>());
        }
    }
}
