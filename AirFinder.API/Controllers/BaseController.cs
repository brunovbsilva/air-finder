﻿using AirFinder.Domain.Common;
using AirFinder.Domain.JWTClaims;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Infra.Security.Constants;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
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

        protected new IActionResult Response(BaseResponse response)
        {
            if (IsValidOperation())
            {
                return Ok(response);
            }

            response = new GenericResponse { 
                Success = false,
                Error = _notification.NotificationModel
            };

            return (_notification?.NotificationModel?.NotificationType) switch
            {
                ENotificationType.NotFound => NotFound(response),
                ENotificationType.BadRequestError => BadRequest(response),
                ENotificationType.Forbidden => Forbid(),
                ENotificationType.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, response),
                ENotificationType.NotAllowed => StatusCode((int)HttpStatusCode.MethodNotAllowed, response),
                _ => StatusCode((int)HttpStatusCode.InternalServerError, response),
            };
        }

        protected Profile GetProfile(HttpContext http)
        {
            var token = http.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var profile = JsonSerializer.Deserialize<Profile>(jsonToken.Claims.FirstOrDefault(x => x.Type == JwtClaims.CAIM_USER_PROFILE)!.Value, serializeOptions);
            return profile!;
        }
    }
}
