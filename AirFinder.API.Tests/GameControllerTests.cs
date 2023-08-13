using AirFinder.Application.Games.Services;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.Games.Models.Responses;

namespace AirFinder.API.Tests
{
    public class GameControllerTests
    {
        readonly TestConfiguration _configuration;
        readonly Mock<IGameService> _gameService;
        readonly Mock<INotification> _notification;
        readonly Mock<HttpContext> _httpContext;
        readonly GameController _controller;

        public GameControllerTests()
        {
            _gameService = new Mock<IGameService>();
            _notification = new Mock<INotification>();
            _httpContext = new Mock<HttpContext>();

            _configuration = new TestConfiguration(_notification, _httpContext);

            _controller = new GameController(_notification.Object, _gameService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = _httpContext.Object }
            };
        }

        #region CreateGame
        [Fact]
        public async Task CreateGame_ShouldReturnOk()
        {
            // Arrange
            var request = new CreateGameRequest();
            var response = new GenericResponse();
            _gameService.Setup(x => x.CreateGame(It.IsAny<CreateGameRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateGame(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task CreateGame_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new CreateGameRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.CreateGame(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region ListGames
        [Fact]
        public async Task ListGames_ShouldReturnOk()
        {
            // Arrange
            var request = new ListGamesRequest();
            var response = new ListGamesResponse();
            _gameService.Setup(x => x.ListGames(It.IsAny<ListGamesRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.ListGames(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task ListGames_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new ListGamesRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.ListGames(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region GetDetails
        [Fact]
        public async Task GetDetails_ShouldReturnOk()
        {
            // Arrange
            var request = Guid.NewGuid();
            var response = new GetDetailsResponse();
            _gameService.Setup(x => x.GetDetails(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.GetDetails(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task GetDetails_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = Guid.NewGuid();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.GetDetails(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region UpdateGame
        [Fact]
        public async Task UpdateGame_ShouldReturnOk()
        {
            // Arrange
            var request = new UpdateGameRequest();
            var response = new GenericResponse();
            _gameService.Setup(x => x.UpdateGame(It.IsAny<UpdateGameRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateGame(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        [InlineData(ENotificationType.NotAllowed)]
        public async Task UpdateGame_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new UpdateGameRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.UpdateGame(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region DeleteGame
        [Fact]
        public async Task DeleteGame_ShouldReturnOk()
        {
            // Arrange
            var request = Guid.NewGuid();
            var response = new GenericResponse();
            _gameService.Setup(x => x.DeleteGame(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteGame(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        [InlineData(ENotificationType.NotAllowed)]
        public async Task DeleteGame_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = Guid.NewGuid();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.DeleteGame(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region JoinGame
        [Fact]
        public async Task JoinGame_ShouldReturnOk()
        {
            // Arrange
            var request = Guid.NewGuid();
            var response = new GenericResponse();
            _gameService.Setup(x => x.JoinGame(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.JoinGame(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        [InlineData(ENotificationType.NotAllowed)]
        public async Task JoinGame_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = Guid.NewGuid();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.JoinGame(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region LeaveGame
        [Fact]
        public async Task LeaveGame_ShouldReturnOk()
        {
            // Arrange
            var request = Guid.NewGuid();
            var response = new GenericResponse();
            _gameService.Setup(x => x.LeaveGame(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.LeaveGame(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        [InlineData(ENotificationType.NotAllowed)]
        public async Task LeaveGame_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = Guid.NewGuid();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.LeaveGame(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region PayGame
        [Fact]
        public async Task PayGame_ShouldReturnOk()
        {
            // Arrange
            var request = Guid.NewGuid();
            var response = new GenericResponse();
            _gameService.Setup(x => x.PayGame(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.PayGame(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task PayGame_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = Guid.NewGuid();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.PayGame(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region ValidateGameJoin
        [Fact]
        public async Task ValidateGameJoin_ShouldReturnOk()
        {
            // Arrange
            var request = new ValidateGameJoinRequest();
            var response = new GenericResponse();
            _gameService.Setup(x => x.ValidateGameJoin(It.IsAny<ValidateGameJoinRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.ValidateGameJoin(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        [InlineData(ENotificationType.NotAllowed)]
        public async Task ValidateGameJoin_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new ValidateGameJoinRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.ValidateGameJoin(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion
    }
}
