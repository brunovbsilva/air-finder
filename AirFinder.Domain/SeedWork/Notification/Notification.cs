using static AirFinder.Domain.SeedWork.Notification.NotificationModel;

namespace AirFinder.Domain.SeedWork.Notification
{
    public class Notification : INotification
    {
        public NotificationModel? _notification;
        public bool HasNotification => _notification != null;
        public NotificationModel? NotificationModel => _notification;
        public void AddNotification(string key, string message, ENotificationType notificationType)
        {
            _notification = new NotificationModel(key, message, notificationType);
        }
    }
}
