using AirFinder.Application.Common;
using AirFinder.Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static AirFinder.Domain.SeedWork.Notification.NotificationModel;

namespace AirFinder.API.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly INotification _notification;
        protected BaseController(INotification notification)
        {
            _notification = notification;
        }

        private bool IsValidOperation() => !_notification.HasNotification;

        protected new IActionResult Response(BaseResponse? response)
        {
            if (IsValidOperation())
            {
                if (response == null) 
                    return NoContent();

                return Ok(response);
            }

            response = new GenericResponse { 
                Success = false,
                Error = _notification.NotificationModel
            };

            switch (_notification?.NotificationModel?.NotificationType)
            {
                case ENotificationType.NotFound:
                    return NotFound(response);
                case ENotificationType.BadRequestError:
                    return BadRequest(response);
                case ENotificationType.Forbidden:
                    return Forbid();
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
