using AirFinder.Application.Battlegrounds.Services;
using AirFinder.Domain.Battlegrounds.Models.Requests;
using AirFinder.Domain.Battlegrounds.Models.Responses;

namespace AirFinder.API.Tests
{
    public class BattlegroundControllerTests
    {
        readonly TestConfiguration _configuration;
        readonly Mock<IBattlegroundService> _battlegroundService;
        readonly Mock<INotification> _notification;
        readonly Mock<HttpContext> _httpContext;
        readonly BattlegroundController _battlegroundController;

        public BattlegroundControllerTests()
        {
            _battlegroundService = new Mock<IBattlegroundService>();
            _notification = new Mock<INotification>();
            _httpContext = new Mock<HttpContext>();

            _configuration = new TestConfiguration(_notification, _httpContext);

            _battlegroundController = new BattlegroundController(_notification.Object, _battlegroundService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = _httpContext.Object }
            };
        }

        #region GetBattlegrounds
        [Fact]
        public async Task GetBattlegrounds_ShouldReturnOk()
        {
            // Arrange
            var battlegrounds = new GetBattlegroundsResponse();
            _battlegroundService.Setup(x => x.GetBattlegrounds(It.IsAny<Guid>())).ReturnsAsync(battlegrounds);

            // Act
            var result = await _battlegroundController.GetBattlegrounds();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task GetBattleGrounds_Errors(ENotificationType notificationType)
        {
            // Arrange
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _battlegroundController.GetBattlegrounds();

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region CreateBattleground
        [Fact]
        public async Task CreateBattleground_ShouldReturnOk()
        {
            // Arrange
            var request = new CreateBattlegroundRequest();
            var response = new GenericResponse();
            _battlegroundService.Setup(x => x.CreateBattleground(It.IsAny<Guid>(), It.IsAny<CreateBattlegroundRequest>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.CreateBattleground(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        #endregion

        #region DeleteBattleground
        [Fact]
        public async Task DeleteBattleground_ShouldReturnOk()
        {
            // Arrange
            var request = Guid.NewGuid();
            var response = new GenericResponse();
            _battlegroundService.Setup(x => x.DeleteBattleground(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.DeleteBattleground(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task DeleteBattleground_Errors(ENotificationType notificationType)
        {
            // Arrange
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _battlegroundController.DeleteBattleground(Guid.NewGuid());

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region UpdateBattleground
        [Fact]
        public async Task UpdateBattleground_ShouldReturnOk()
        {
            // Arrange
            var requestGuid = Guid.NewGuid();
            var request = new UpdateBattlegroundRequest();
            var response = new GenericResponse();
            _battlegroundService.Setup(x => x.UpdateBattleground(It.IsAny<Guid>(), It.IsAny<UpdateBattlegroundRequest>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.UpdateBattleground(requestGuid, request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task UpdateBattleground_Errors(ENotificationType notificationType)
        {
            // Arrange
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _battlegroundController.UpdateBattleground(Guid.NewGuid(), new UpdateBattlegroundRequest());

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion
    }
}
