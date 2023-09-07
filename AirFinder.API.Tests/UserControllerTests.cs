using AirFinder.Application.Users.Services;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Domain.Users.Models.Responses;

namespace AirFinder.API.Tests
{
    public class UserControllerTests
    {
        readonly TestConfiguration _configuration;
        readonly Mock<IUserService> _userService;
        readonly Mock<INotification> _notification;
        readonly Mock<HttpContext> _httpContext;
        readonly UserController _controller;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _notification = new Mock<INotification>();
            _httpContext = new Mock<HttpContext>();
            _configuration = new TestConfiguration(_notification, _httpContext);

            _controller = new UserController(_notification.Object, _userService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext.Object
                }
            };
        }

        #region Login
        [Fact]
        public async Task Login_ShouldReturnOk()
        {
            // Arrange
            var request = new LoginRequest();
            var response = new LoginResponse();
            _userService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task Login_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new LoginRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.Login(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region CreateUser
        [Fact]
        public async Task CreateUser_ShouldReturnOk()
        {
            // Arrange
            var request = new UserRequest();
            var response = new GenericResponse();
            _userService.Setup(x => x.CreateUserAsync(It.IsAny<UserRequest>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.NotAllowed)]
        public async Task CreateUser_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new UserRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region CreateAnotherUser
        [Fact]
        public async Task CreateUserAdmin_ShouldReturnOk()
        {
            // Arrange
            var request = new UserAdminRequest();
            var response = new GenericResponse();
            _userService.Setup(x => x.CreateUserAdminAsync(It.IsAny<UserAdminRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateAnotherUser(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.NotAllowed)]
        [InlineData(ENotificationType.Forbidden)]
        public async Task CreateUserAdmin_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new UserAdminRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.CreateAnotherUser(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
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
            var result = await _controller.Delete();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task DeleteUser_Errors(ENotificationType notificationType)
        {
            // Arrange
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.Delete();

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
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
            var result = await _controller.Put(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task UpdatePassword_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new UpdatePasswordRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.Put(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
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

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task SendTokenForgotPassword_Errors(ENotificationType notificationType)
        {
            // Arrange
            var email = "test@example.com";
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.SendTokenForgotPassword(email);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
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

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task VerifyToken_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new VerifyTokenRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.VerifyToken(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
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

        [Theory]
        [InlineData(ENotificationType.BadRequestError)]
        public async Task ChangePassword_Errors(ENotificationType notificationType)
        {
            // Arrange
            var request = new ChangePasswordRequest();
            _configuration.SetupNotification(notificationType);

            // Act
            var result = await _controller.UpdatePassword(request);

            // Assert
            _configuration.NotificationsAsserts(notificationType, result);
        }
        #endregion
    }
}
