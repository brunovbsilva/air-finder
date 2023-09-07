using AirFinder.Domain.GameLogs;

namespace AirFinder.Application.Tests.Mocks
{
    public class GameLogMocks
    {
        public static GameLog Default()
        {
            return new GameLog(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()
            )
            {
                Id = It.IsAny<Guid>()
            };
        }
        public static IEnumerable<GameLog> DefaultEnumerable()
        {
            return new EnumerableQuery<GameLog>(new List<GameLog> { Default() });
        }
        public static IEnumerable<GameLog> DefaultEmptyEnumerable()
        {
            return new EnumerableQuery<GameLog>(new List<GameLog>());
        }
    }
}
