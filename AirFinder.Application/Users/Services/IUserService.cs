using AirFinder.Application.Common;
using AirFinder.Application.Users.Models.Request;
using AirFinder.Application.Users.Models.Response;
using AirFinder.Domain.Users;

namespace AirFinder.Application.Users.Services
{
    public interface IUserService
    {
        Task Insert(User item);
        Task<BaseResponse?> Delete(Guid id);
        Task<CreateUserResponse?> CreateUserAsync(UserRequest request);
        Task<LoginResponse?> Login(string login, string password);
        Task<BaseResponse?> UpdatePasswordAsync(Guid id, UpdatePasswordRequest password);
        Task<BaseResponse?> SendTokenEmail(string email);
        Task<BaseResponse?> VerifyToken(VerifyTokenRequest request);
        Task<BaseResponse?> ChangePassword(ChangePasswordRequest request);
    }
}
