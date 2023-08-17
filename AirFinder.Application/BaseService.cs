using AirFinder.Application.Email.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Infra.Utils.Constants;
using SendGrid.Helpers.Errors.Model;

namespace AirFinder.Application
{
    public abstract class BaseService
    {
        protected readonly INotification _notification;
        public BaseService(INotification notification)
        {
            _notification = notification;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (NotFoundException e)
            {
                _notification.AddNotification(NotificationKeys.NotFound, e.Message, NotificationModel.ENotificationType.NotFound);
            }
            catch (ArgumentException e)
            {
                _notification.AddNotification(NotificationKeys.InvalidProperty, e.Message, NotificationModel.ENotificationType.BadRequestError);
            }
            catch (ForbiddenException e)
            {
                _notification.AddNotification(NotificationKeys.Forbidden, e.Message, NotificationModel.ENotificationType.Forbidden);
            }
            catch (MethodNotAllowedException e)
            {
                _notification.AddNotification(NotificationKeys.MethodNotAllowed, e.Message, NotificationModel.ENotificationType.NotAllowed);
            }
            catch (Exception e)
            {
                _notification.AddNotification(NotificationKeys.InternalError, e.Message, NotificationModel.ENotificationType.InternalServerError);
            }
            return default!;
        }
    }
}
