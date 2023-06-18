using AirFinder.Domain.Common;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Domain.Users.Models.Responses;

namespace AirFinder.Application.Users.Services
{
    public interface IUserService
    {
        Task<BaseResponse?> Delete(Guid id);
        Task<CreateUserResponse?> CreateUserAsync(UserRequest request);
        Task<LoginResponse?> Login(string login, string password);
        Task<BaseResponse?> UpdatePasswordAsync(Guid id, UpdatePasswordRequest password);
        Task<BaseResponse?> SendTokenEmail(string email);
        Task<BaseResponse?> VerifyToken(VerifyTokenRequest request);
        Task<BaseResponse?> ChangePassword(ChangePasswordRequest request);
        Task<BaseResponse?> CreateAnotherUserAsync(CreateAnotherUserRequest request, Guid userId); 
    }
}
