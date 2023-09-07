using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Infra.Utils.Constants;

namespace AirFinder.Application.Tests.Configuration
{
    public class NotificationAssert
    {
        public static void BadRequestNotification(Mock<INotification> _notification)
        {
            _notification.Verify(x =>
                x.AddNotification(NotificationKeys.InvalidProperty, It.IsAny<string>(), NotificationModel.ENotificationType.BadRequestError),
                Times.Once
            );
        }
        public static void ForbiddenNotification(Mock<INotification> _notification)
        {
            _notification.Verify(x =>
                x.AddNotification(NotificationKeys.Forbidden, It.IsAny<string>(), NotificationModel.ENotificationType.Forbidden),
                Times.Once
            );
        }
        public static void MethodNotAllowedNotification(Mock<INotification> _notification)
        {
            _notification.Verify(x =>
                x.AddNotification(NotificationKeys.MethodNotAllowed, It.IsAny<string>(), NotificationModel.ENotificationType.NotAllowed),
                Times.Once
            );
        }
    }
}
