﻿using AirFinder.Application.Users.Services;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirFinder.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(INotification notification, IUserService userService) : base(notification) 
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] string login, [FromQuery] string password)
        {
            return Response(await _userService.Login(login, password));
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            return Response(await _userService.CreateUserAsync(request));
        }
        [Authorize]
        [HttpPost("another")]
        public async Task<IActionResult> CreateAnotherUser([FromBody] CreateAnotherUserRequest request)
        {
            return Response(await _userService.CreateAnotherUserAsync(request, GetUserId(HttpContext)));
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Response(await _userService.Delete(id));
        }
        [Authorize]
        [HttpPut("password/{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdatePasswordRequest request)
        {
            return Response(await _userService.UpdatePasswordAsync(id, request));
        }

        [HttpPost("Password/token")]
        public async Task<IActionResult> SendTokenForgotPassword([FromQuery] string email)
        {
            return Response(await _userService.SendTokenEmail(email));
        }
        [HttpGet("Password/token")]
        public async Task<IActionResult> VerifyToken([FromQuery] VerifyTokenRequest request) 
        {
            return Response(await _userService.VerifyToken(request));
        }
        [HttpPut("Password/token")]
        public async Task<IActionResult> UpdatePassword([FromQuery] ChangePasswordRequest request)
        {
            return Response(await _userService.ChangePassword(request));
        }
    }
}
