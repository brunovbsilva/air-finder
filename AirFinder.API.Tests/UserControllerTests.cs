using AirFinder.Application.Users.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Domain.Users.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Any;
using Microsoft.VisualBasic;
using System.Net.Http;
using static AirFinder.Domain.SeedWork.Notification.NotificationModel;

namespace AirFinder.API.Tests
{
    public class UserControllerTests
    {
        readonly Mock<IUserService> _userService;
        readonly Mock<INotification> _notification;
        readonly Mock<HttpContext> _httpContext;
        readonly UserController _controller;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _notification = new Mock<INotification>();
            _httpContext = new Mock<HttpContext>();
            SetupHttpContext();

            _controller = new UserController(_notification.Object, _userService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext.Object
                }
            };
        }

        #region Notifications
        [Theory]
        [InlineData(ENotificationType.Default)]
        [InlineData(ENotificationType.NotFound)]
        [InlineData(ENotificationType.BadRequestError)]
        [InlineData(ENotificationType.Forbidden)]
        [InlineData(ENotificationType.InternalServerError)]
        public async Task Notification(ENotificationType notificationType)
        {
            // Arrange
            var login = "login";
            var password = "password";
            SetupNotification(notificationType);

            // Act
            var result = await _controller.Login(login, password);

            // Assert
            NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region Login
        [Fact]
        public async Task Login_ShouldReturnOk()
        {
            // Arrange
            var login = "login";
            var password = "password";
            var response = new LoginResponse();
            _userService.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

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
            LoginResponse? response = null;
            _userService.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await _controller.Login(login, password);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region CreateUser
        [Fact]
        public async Task CreateUser_ShouldReturnOk()
        {
            // Arrange
            var request = new UserRequest();
            var response = new CreateUserResponse();
            _userService.Setup(x => x.CreateUserAsync(It.IsAny<UserRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_NoContent()
        {
            // Arrange
            var request = new UserRequest();
            CreateUserResponse? response = null;
            _userService.Setup(x => x.CreateUserAsync(It.IsAny<UserRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region CreateAnotherUser
        [Fact]
        public async Task CreateAnotherUser_ShouldReturnOk()
        {
            // Arrange
            var request = new CreateAnotherUserRequest();
            var response = new CreateUserResponse();
            _userService.Setup(x => x.CreateAnotherUserAsync(It.IsAny<CreateAnotherUserRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateAnotherUser(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateAnotherUser_NoContent()
        {
            // Arrange
            var request = new CreateAnotherUserRequest();
            CreateUserResponse? response = null;
            _userService.Setup(x => x.CreateAnotherUserAsync(It.IsAny<CreateAnotherUserRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateAnotherUser(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteUser_ShouldReturnOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new GenericResponse();
            _userService.Setup(x => x.DeleteUserAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_NoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            GenericResponse? response = null;
            _userService.Setup(x => x.DeleteUserAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region UpdatePassword
        [Fact]
        public async Task UpdatePassword_ShouldReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdatePasswordRequest();
            var response = new GenericResponse();
            _userService.Setup(x => x.UpdatePasswordAsync(It.IsAny<Guid>(), It.IsAny<UpdatePasswordRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.Put(id, request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePassword_NoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdatePasswordRequest();
            GenericResponse? response = null;
            _userService.Setup(x => x.UpdatePasswordAsync(It.IsAny<Guid>(), It.IsAny<UpdatePasswordRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.Put(id, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region SendTokenForgotPassword
        [Fact]
        public async Task SendTokenForgotPassword_ShouldReturnOk()
        {
            // Arrange
            var email = "test@example.com";
            var response = new GenericResponse();
            _userService.Setup(x => x.SendTokenEmailAsync(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await _controller.SendTokenForgotPassword(email);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task SendTokenForgotPassword_NoContent()
        {
            // Arrange
            var email = "test@example.com";
            GenericResponse? response = null;
            _userService.Setup(x => x.SendTokenEmailAsync(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await _controller.SendTokenForgotPassword(email);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region VerifyToken
        [Fact]
        public async Task VerifyToken_ShouldReturnOk()
        {
            // Arrange
            var request = new VerifyTokenRequest();
            var response = new GenericResponse();
            _userService.Setup(x => x.VerifyTokenAsync(It.IsAny<VerifyTokenRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.VerifyToken(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task VerifyToken_NoContent()
        {
            // Arrange
            var request = new VerifyTokenRequest();
            GenericResponse? response = null;
            _userService.Setup(x => x.VerifyTokenAsync(It.IsAny<VerifyTokenRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.VerifyToken(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region ChangePassword
        [Fact]
        public async Task ChangePassword_ShouldReturnOk()
        {
            // Arrange
            var request = new ChangePasswordRequest();
            var response = new GenericResponse();
            _userService.Setup(x => x.ChangePasswordAsync(It.IsAny<ChangePasswordRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdatePassword(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ChangePassword_NoContent()
        {
            // Arrange
            var request = new ChangePasswordRequest();
            GenericResponse? response = null;
            _userService.Setup(x => x.ChangePasswordAsync(It.IsAny<ChangePasswordRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.UpdatePassword(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region private methods
        private void SetupHttpContext()
        {
            var userId = Guid.NewGuid();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJlY2U1YTI5ZS1hY2Y1LTQ5OTMtOTk5MC0wOTM1OGU4MzA3MjQiLCJuYmYiOjAsImV4cCI6MCwiaWF0IjowfQ.hbZd8yUayA6mNd20e4VrPm0KAcshCpqO7-VFFD5i35I";
            _httpContext.Setup(x => x.Request.Headers["Authorization"]).Returns($"Bearer {token}");
            var claims = new[] { new System.Security.Claims.Claim("userId", userId.ToString()) };
        }

        private void SetupNotification(ENotificationType notificationType)
        {
            var notificationModel = new NotificationModel("mocked key", "mocked message", notificationType);
            _notification.Setup(x => x.HasNotification).Returns(true);
            _notification.Setup(x => x.NotificationModel).Returns(notificationModel);
        }

        private static void NotificationsAsserts(ENotificationType notificationType, IActionResult result)
        {
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
