using AirFinder.Application.Users.Services;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users.Models.Responses;
using static AirFinder.Domain.SeedWork.Notification.NotificationModel;

namespace AirFinder.API.Tests
{
    public class UserControllerTests
    {
        readonly Mock<IUserService> _userService;
        readonly Mock<INotification> _notification;
        readonly UserController _controller;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _notification = new Mock<INotification>();
            _controller = new UserController(_notification.Object, _userService.Object);
        }

        #region Login

        [Fact]
        public async Task Login_ShouldReturnOk()
        {
            // Arrange
            var login = "login";
            var password = "password";
            LoginResponse response = new LoginResponse();
            _userService.Setup(x => x.Login(login, password)).ReturnsAsync(response);

            // Act
            var result = await _controller.Login(login, password);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_NoContent()
        {
            // Arrange
            var login = "login";
            var password = "password";
            _userService.Setup(x => x.Login(login, password)).ReturnsAsync((LoginResponse)null);

            // Act
            var result = await _controller.Login(login, password);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.Default)]
        [InlineData(ENotificationType.NotFound)]
        [InlineData(ENotificationType.BadRequestError)]
        [InlineData(ENotificationType.Forbidden)]
        public async Task Login_Notification(ENotificationType notificationType)
        {
            // Arrange
            var login = "login";
            var password = "password";
            var notificationModel = new NotificationModel("mocked key", "mocked message", notificationType);
            _notification.Setup(x => x.HasNotification).Returns(true);
            _notification.Setup(x => x.NotificationModel).Returns(notificationModel);

            // Act
            var result = await _controller.Login(login, password);

            // Assert
            switch (notificationType)
            {
                case ENotificationType.NotFound:
                    Assert.IsType<NotFoundObjectResult>(result);
                    break;
                case ENotificationType.BadRequestError:
                    Assert.IsType<BadRequestObjectResult>(result);
                    break;
                case ENotificationType.Forbidden:
                    Assert.IsType<ForbidResult>(result);
                    break;
                default:
                    Assert.IsType<ObjectResult>(result);
                    break;
            }
        }

        #endregion
    }
}
