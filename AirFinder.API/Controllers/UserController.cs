using AirFinder.Application.Users.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Domain.Users.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Login to API")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromQuery] LoginRequest request)
        {
            return Response(await _userService.LoginAsync(request));
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            return Response(await _userService.CreateUserAsync(request));
        }

        [Authorize]
        [HttpPost("admin")]
        [SwaggerOperation(Summary = "Create a new user with role")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> CreateAnotherUser([FromBody] UserAdminRequest request)
        {
            return Response(await _userService.CreateUserAdminAsync(request, GetProfile(HttpContext).UserId));
        }

        [Authorize]
        [HttpDelete()]
        [SwaggerOperation(Summary = "Delete a user")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete()
        {
            return Response(await _userService.DeleteUserAsync(GetProfile(HttpContext).UserId));
        }
        [Authorize]
        [HttpPut("password")]
        [SwaggerOperation(Summary = "Update password")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdatePasswordRequest request)
        {
            return Response(await _userService.UpdatePasswordAsync(GetProfile(HttpContext).UserId, request));
        }

        [HttpPost("Password/token")]
        [SwaggerOperation(Summary = "Send token to email")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendTokenForgotPassword([FromQuery] string email)
        {
            return Response(await _userService.SendTokenEmailAsync(email));
        }

        [HttpGet("Password/token")]
        [SwaggerOperation(Summary = "Verify token")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyToken([FromQuery] VerifyTokenRequest request) 
        {
            return Response(await _userService.VerifyTokenAsync(request));
        }

        [HttpPut("Password/token")]
        [SwaggerOperation(Summary = "Update password from token")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePassword([FromQuery] ChangePasswordRequest request)
        {
            return Response(await _userService.ChangePasswordAsync(request));
        }
    }
}
