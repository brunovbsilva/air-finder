using AirFinder.Application.Common;
using AirFinder.Application.Users.Models.Request;
using AirFinder.Application.Users.Models.Response;
using AirFinder.Domain.Users;

namespace AirFinder.Application.Users.Services
{
    public interface IUserService : IGenericService<User>
    {
        Task<User> GetByLoginAsync(string login);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByCPFAsync(string cpf);
        Task<CreateUserResponse> CreateUserAsync(UserRequest request);
        Task<LoginResponse> Login(string login, string password);
        
        Task<BaseResponse> UpdatePasswordAsync(int id, UpdatePasswordRequest password);
        #region Change Password
        Task<BaseResponse> SendTokenEmail(string email);
        Task<BaseResponse> VerifyToken(VerifyTokenRequest request);
        Task<BaseResponse> ChangePassword(ChangePasswordRequest request);
        #endregion
    }
}
