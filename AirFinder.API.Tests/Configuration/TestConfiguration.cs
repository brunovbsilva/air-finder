using AirFinder.API.Tests.Constants;

namespace AirFinder.API.Tests.Configuration
{
    public class TestConfiguration
    {
        private readonly Mock<HttpContext> _httpContext;
        private readonly Mock<INotification> _notification;
        public TestConfiguration(
            Mock<INotification> notification, 
            Mock<HttpContext> httpContext
        )
        {
            _notification = notification;
            _httpContext = httpContext;

            SetupHttpContext();
        }
        private void SetupHttpContext()
        {
            var userId = Guid.NewGuid();
            var token = GenericMocks._tokenJWT;
            _httpContext.Setup(x => x.Request.Headers["Authorization"]).Returns($"Bearer {token}");
            var claims = new[] { new System.Security.Claims.Claim("userId", userId.ToString()) };
        }

        public void SetupNotification(ENotificationType notificationType)
        {
            var notificationModel = new NotificationModel(GenericMocks._string, GenericMocks._string, notificationType);
            _notification.Setup(x => x.HasNotification).Returns(true);
            _notification.Setup(x => x.NotificationModel).Returns(notificationModel);
        }

        public void NotificationsAsserts(ENotificationType notificationType, IActionResult result)
        {
            switch (notificationType)
            {
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
    }
}
