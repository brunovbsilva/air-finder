using AirFinder.Application.Email.Services;
using AirFinder.Domain.SeedWork.Notification;
using SendGrid.Helpers.Errors.Model;

namespace AirFinder.Application
{
    public abstract class BaseService
    {
        protected readonly INotification _notification;
        protected readonly IMailService _mailService;
        public BaseService(INotification notification, IMailService mailService)
        {
            _notification = notification;
            _mailService = mailService;
        }

        public async Task<T?> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (NotFoundException e)
            {
                _notification.AddNotification("Not Found", e.Message, NotificationModel.ENotificationType.NotFound);
            }
            catch (ArgumentException e)
            {
                _notification.AddNotification("Invalid Property", e.Message, NotificationModel.ENotificationType.BadRequestError);
            }
            catch (ForbiddenException e)
            {
                _notification.AddNotification("Forbidden", e.Message, NotificationModel.ENotificationType.Forbidden);
            }
            catch (Exception e)
            {
                _notification.AddNotification("Internal Error", e.Message, NotificationModel.ENotificationType.InternalServerError);
            }
            return default;
        }

    }
}
