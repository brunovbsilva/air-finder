using static AirFinder.Domain.SeedWork.Notification.NotificationModel;

namespace AirFinder.Domain.SeedWork.Notification
{
    public interface INotification
    {
        NotificationModel? NotificationModel { get; }
        bool HasNotification { get; }
        void AddNotification(string key, string message, ENotificationType notificationType);
    }
}
