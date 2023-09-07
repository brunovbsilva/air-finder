using AirFinder.Application.Games.Services;
using AirFinder.Application.Tests.Configuration;
using AirFinder.Application.Tests.Enums;
using AirFinder.Application.Tests.Mocks;
using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.Common;
using AirFinder.Domain.GameLogs;
using AirFinder.Domain.Games;
using AirFinder.Domain.Games.Models.Dtos;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.Games.Models.Responses;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Infra.Data.Repository;
using System.Linq.Expressions;

namespace AirFinder.Application.Tests
{
    public class GameServiceTests
    {
        private readonly Mock<INotification> _notification;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IGameRepository> _gameRepository;
        private readonly Mock<IBattlegroundRepository> _battlegroundRepository;
        private readonly Mock<IGameLogRepository> _gameLogRepository;
        private readonly GameService _service;

        public GameServiceTests()
        {
            _notification = new Mock<INotification>();
            _userRepository = new Mock<IUserRepository>();
            _gameRepository = new Mock<IGameRepository>();
            _battlegroundRepository = new Mock<IBattlegroundRepository>();
            _gameLogRepository = new Mock<IGameLogRepository>();
            _service = new GameService(
                _notification.Object,
                _userRepository.Object,
                _gameRepository.Object,
                _battlegroundRepository.Object,
                _gameLogRepository.Object
            );
        }

        #region CreateGame
        [Fact]
        public async Task CreateGameAsync_ShouldCreate()
        {
            // Arrange
            var request = new CreateGameRequest();

            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _battlegroundRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Battleground, bool>>>())).ReturnsAsync(true);

            // Act
            var result = await _service.CreateGame(request, It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Theory]
        [InlineData(GameException.NotFoundUserException)]
        [InlineData(GameException.NotFoundBattlegroundException)]
        public async Task CreateGameAsync_Exception(GameException exception)
        {
            // Arrange
            var request = new CreateGameRequest();

            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundUserException);
            _battlegroundRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Battleground, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundBattlegroundException);

            // Act
            var result = await _service.CreateGame(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region ListGames
        [Fact]
        public async Task ListGamesAsync_ShouldList()
        {
            // Arrange
            var request = new ListGamesRequest();
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _gameRepository.Setup(x => x.getGameList(It.IsAny<ListGamesRequest>(), It.IsAny<Guid>())).ReturnsAsync(new ListGamesResponse());

            // Act
            var result = await _service.ListGames(request, It.IsAny<Guid>());

            // Assert
            Assert.IsType<ListGamesResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Fact]
        public async Task ListGamesAsync_Exception()
        {
            // Arrange
            var request = new ListGamesRequest();
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(false);

            // Act
            var result = await _service.ListGames(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region GetDetails
        [Fact]
        public async Task GetDetailsAsync_ShouldGetDetails()
        {
            // Arrange
            _gameRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(GameMocks.Default());

            // Act
            var result = await _service.GetDetails(It.IsAny<Guid>());

            // Assert
            Assert.IsType<GetDetailsResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Fact]
        public async Task GetDetailsAsync_Exception()
        {
            // Act
            var result = await _service.GetDetails(It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region UpdateGame
        [Fact]
        public async Task UpdateGameAsync_ShouldUpdate()
        {
            // Arrange
            var request = new UpdateGameRequest();
            _gameRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(GameMocks.DefaultEnumerable().BuildMock());

            // Act
            var result = await _service.UpdateGame(request, It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Fact]
        public async Task UpdateGameAsync_Exception()
        {
            // Arrange
            var request = new UpdateGameRequest();
            _gameRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(GameMocks.DefaultEmptyEnumerable().BuildMock());

            // Act
            var result = await _service.UpdateGame(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region DeleteGame
        [Fact]
        public async Task DeleteGameAsync_ShouldDelete()
        {
            // Arrange
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteGame(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Theory]
        [InlineData(GameException.NotFoundUserException)]
        [InlineData(GameException.NotFoundGameException)]
        public async Task DeleteGameAsync_Exception(GameException exception)
        {
            // Arrange
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundUserException);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundGameException);

            // Act
            var result = await _service.DeleteGame(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region JoinGame
        [Fact]
        public async Task JoinGameAsync_ShouldJoin()
        {
            // Arrange
            var request = new JoinGameRequest();
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(true);

            // Act
            var result = await _service.JoinGame(request, It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Theory]
        [InlineData(GameException.NotFoundUserException)]
        [InlineData(GameException.NotFoundGameException)]
        public async Task JoinGameAsync_Exception(GameException exception)
        {
            // Arrange
            var request = new JoinGameRequest();
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundUserException);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundGameException);

            // Act
            var result = await _service.JoinGame(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region LeaveGame
        [Fact]
        public async Task LeaveGameAsync_ShouldLeave()
        {
            // Arrange
            var gameLog = GameLogMocks.DefaultEnumerable();
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(true);
            _gameLogRepository.Setup(x => x.GetAll()).Returns(gameLog.BuildMock());

            // Act
            var result = await _service.LeaveGame(gameLog.FirstOrDefault()!.GameId, gameLog.FirstOrDefault()!.UserId);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Theory]
        [InlineData(GameException.NotFoundUserException)]
        [InlineData(GameException.NotFoundGameException)]
        [InlineData(GameException.NotFoundGameLogException)]
        public async Task LeaveGameAsync_Exception(GameException exception)
        {
            // Arrange
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundUserException);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundGameException);
            _gameLogRepository.Setup(x => x.GetAll()).Returns(
                exception == GameException.NotFoundGameLogException ?
                GameLogMocks.DefaultEmptyEnumerable().BuildMock() :
                GameLogMocks.DefaultEnumerable().BuildMock()
            );

            // Act
            var result = await _service.LeaveGame(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region PayGame
        [Fact]
        public async Task PayGameAsync_ShouldPay()
        {
            // Arrange
            var gameLog = GameLogMocks.DefaultEnumerable();
            var request = new PayGameRequest()
            {
                GameId = gameLog.FirstOrDefault()!.GameId
            };
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(true);
            _gameLogRepository.Setup(x => x.GetAll()).Returns(gameLog.BuildMock());

            // Act
            var result = await _service.PayGame(request, gameLog.FirstOrDefault()!.UserId);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Theory]
        [InlineData(GameException.NotFoundUserException)]
        [InlineData(GameException.NotFoundGameException)]
        [InlineData(GameException.NotFoundGameLogException)]
        public async Task PayGameAsync_Exception(GameException exception)
        {
            // Arrange
            var gameLog = exception == GameException.NotFoundGameLogException ? GameLogMocks.DefaultEmptyEnumerable() :
                GameLogMocks.DefaultEnumerable();
            var request = new PayGameRequest()
            {
                GameId = gameLog.FirstOrDefault()?.GameId ?? It.IsAny<Guid>()
            };
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundUserException);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundGameException);
            _gameLogRepository.Setup(x => x.GetAll()).Returns(gameLog.BuildMock());

            // Act
            var result = await _service.PayGame(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region ValidateGameJoin
        [Fact]
        public async Task ValidateGameJoinAsync_ShouldValidate()
        {
            // Arrange
            var gameLog = GameLogMocks.DefaultPayedEnumerable();
            var request = new ValidateGameJoinRequest()
            {
                GameId = gameLog.FirstOrDefault()!.GameId,
                UserId = gameLog.FirstOrDefault()!.UserId
            };
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(true);
            _gameLogRepository.Setup(x => x.GetAll()).Returns(gameLog.BuildMock());

            // Act
            var result = await _service.ValidateGameJoin(request, It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Theory]
        [InlineData(GameException.NotFoundUserException)]
        [InlineData(GameException.NotFoundGameException)]
        [InlineData(GameException.NotFoundGameLogException)]
        [InlineData(GameException.NotPayedException)]
        public async Task ValidateGameJoinAsync_Exception(GameException exception)
        {
            // Arrange
            var gameLog = exception == GameException.NotFoundGameLogException ? GameLogMocks.DefaultEmptyEnumerable() :
                exception == GameException.NotPayedException ? GameLogMocks.DefaultEnumerable() :
                GameLogMocks.DefaultPayedEnumerable();
            var request = new ValidateGameJoinRequest()
            {
                GameId = gameLog.FirstOrDefault()?.GameId ?? It.IsAny<Guid>(),
                UserId = gameLog.FirstOrDefault()?.UserId ?? It.IsAny<Guid>()
            };
            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundUserException);
            _gameRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(exception != GameException.NotFoundGameException);
            _gameLogRepository.Setup(x => x.GetAll()).Returns(gameLog.BuildMock());

            // Act
            var result = await _service.ValidateGameJoin(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            if(exception == GameException.NotPayedException)
                NotificationAssert.MethodNotAllowedNotification(_notification);
            else
                NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion
    }
}
