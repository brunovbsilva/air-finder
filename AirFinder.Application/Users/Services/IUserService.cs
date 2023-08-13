using AirFinder.Domain.Common;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Domain.Users.Models.Responses;

namespace AirFinder.Application.Users.Services
{
    public interface IUserService
    {
        Task<BaseResponse?> DeleteUserAsync(Guid id);
        Task<CreateUserResponse?> CreateUserAsync(UserRequest request);
        Task<LoginResponse?> LoginAsync(string login, string password);
        Task<BaseResponse?> UpdatePasswordAsync(Guid id, UpdatePasswordRequest password);
        Task<BaseResponse?> SendTokenEmailAsync(string email);
        Task<BaseResponse?> VerifyTokenAsync(VerifyTokenRequest request);
        Task<BaseResponse?> ChangePasswordAsync(ChangePasswordRequest request);
        Task<BaseResponse?> CreateAnotherUserAsync(CreateAnotherUserRequest request, Guid userId); 
    }
}
