using AirFinder.Application.BattleGrounds.Services;
using AirFinder.Domain.BattleGrounds.Models.Requests;
using AirFinder.Domain.BattleGrounds.Models.Responses;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static AirFinder.Domain.SeedWork.Notification.NotificationModel;

namespace AirFinder.API.Tests
{
    public class BattlegroundControllerTests
    {
        readonly Mock<IBattleGroundService> _battleGroundService;
        readonly Mock<INotification> _notification;
        readonly Mock<HttpContext> _httpContext;
        readonly BattlegroundController _battlegroundController;

        public BattlegroundControllerTests()
        {
            _battleGroundService = new Mock<IBattleGroundService>();
            _notification = new Mock<INotification>();
            _httpContext = new Mock<HttpContext>();
            SetupHttpContext();

            _battlegroundController = new BattlegroundController(_notification.Object, _battleGroundService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = _httpContext.Object }
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
            SetupNotification(notificationType);

            // Act
            var result = await _battlegroundController.GetBattlegrounds();

            // Assert
            NotificationsAsserts(notificationType, result);
        }
        #endregion

        #region GetBattlegrounds
        [Fact]
        public async Task GetBattlegrounds_ShouldReturnOk()
        {
            // Arrange
            var battleGrounds = new GetBattleGroundResponse();
            _battleGroundService.Setup(x => x.GetBattleGrounds(It.IsAny<Guid>())).ReturnsAsync(battleGrounds);

            // Act
            var result = await _battlegroundController.GetBattlegrounds();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetBattlegrounds_NoContent()
        {
            // Arrange
            GetBattleGroundResponse? battleGrounds = null;
            _battleGroundService.Setup(x => x.GetBattleGrounds(It.IsAny<Guid>())).ReturnsAsync(battleGrounds);

            // Act
            var result = await _battlegroundController.GetBattlegrounds();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region CreateBattleground
        [Fact]
        public async Task CreateBattleground_ShouldReturnOk()
        {
            // Arrange
            var request = new CreateBattleGroundRequest();
            var response = new GenericResponse();
            _battleGroundService.Setup(x => x.CreateBattleGround(It.IsAny<Guid>(), It.IsAny<CreateBattleGroundRequest>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.CreateBattleground(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateBattleground_NoContent()
        {
            // Arrange
            var request = new CreateBattleGroundRequest();
            BaseResponse? response = null;
            _battleGroundService.Setup(x => x.CreateBattleGround(It.IsAny<Guid>(), It.IsAny<CreateBattleGroundRequest>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.CreateBattleground(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region DeleteBattleground
        [Fact]
        public async Task DeleteBattleground_ShouldReturnOk()
        {
            // Arrange
            var request = Guid.NewGuid();
            var response = new GenericResponse();
            _battleGroundService.Setup(x => x.DeleteBattleGround(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.DeleteBattleground(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteBattleground_NoContent()
        {
            // Arrange
            var request = Guid.NewGuid();
            BaseResponse? response = null;
            _battleGroundService.Setup(x => x.DeleteBattleGround(It.IsAny<Guid>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.DeleteBattleground(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        #endregion

        #region UpdateBattleground
        [Fact]
        public async Task UpdateBattleground_ShouldReturnOk()
        {
            // Arrange
            var requestGuid = Guid.NewGuid();
            var request = new UpdateBattleGroundRequest();
            var response = new GenericResponse();
            _battleGroundService.Setup(x => x.UpdateBattleGround(It.IsAny<Guid>(), It.IsAny<UpdateBattleGroundRequest>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.UpdateBattleground(requestGuid, request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateBattleground_NoContent()
        {
            // Arrange
            var requestGuid = Guid.NewGuid();
            var request = new UpdateBattleGroundRequest();
            BaseResponse? response = null;
            _battleGroundService.Setup(x => x.UpdateBattleGround(It.IsAny<Guid>(), It.IsAny<UpdateBattleGroundRequest>())).ReturnsAsync(response);

            // Act
            var result = await _battlegroundController.UpdateBattleground(requestGuid, request);

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
