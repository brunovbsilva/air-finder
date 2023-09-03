using AirFinder.Domain.Email.Models.Requests;
using AirFinder.Application.Email.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.People;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Tokens;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Domain.Users.Models.Responses;
using AirFinder.Infra.Security;
using AirFinder.Infra.Security.Request;
using AirFinder.Infra.Utils.Constants;
using SendGrid.Helpers.Errors.Model;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Application.Users.Services
{
    public class UserService : BaseService, IUserService
    {
        readonly IUserRepository _userRepository;
        readonly IPersonRepository _personRepository;
        readonly ITokenRepository _tokenRepository;
        readonly IJwtService _jwtService;
        readonly IMailService _mailService;

        public UserService(
            INotification notification,
            IUserRepository userRepository,
            IPersonRepository personRepository,
            ITokenRepository tokenRepository,
            IJwtService jwtService,
            IMailService mailService
        ) : base(notification)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _tokenRepository = tokenRepository;
            _jwtService = jwtService;
            _mailService = mailService;
        }
        
        #region CreateUserAsync
        public async Task<BaseResponse> CreateUserAsync(UserRequest request)
        => await ExecuteAsync(async () =>
        {
            await InsertAsync(new User(request));
            return new GenericResponse();
        });
        #endregion

        #region CreateUserAdminAsync
        public async Task<BaseResponse> CreateUserAdminAsync(UserAdminRequest request, Guid userId)
        => await ExecuteAsync(async () =>
        {
            if (await _userRepository.Get(x => x.Id == userId && x.Roll == UserRoll.Admnistrator).FirstOrDefaultAsync() == null) throw new ForbiddenException();

            await InsertAsync(new User(request));
            return new GenericResponse();
        });
        #endregion

        #region LoginAsync
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.Get(x => x.Login.ToLower() == request.Login.ToLower()).FirstOrDefaultAsync();
            if (user == null || user.Password != request.Password) throw new WrongCredentialsException();

            var tokenRequest = new CreateTokenRequest
            {
                Login = user.Login,
                UserId = user.Id,
                Name = user.Person!.Name,
                Scopes = new List<string>
                {
                    user.Roll == UserRoll.Admnistrator ? "Adm_Roll" : "User_Roll"
                }
            };

            return new LoginResponse
            {
                Token = _jwtService.CreateToken(tokenRequest)
            };
        });
        #endregion

        #region UpdatePassword
        public async Task<BaseResponse> UpdatePasswordAsync(Guid id, UpdatePasswordRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIDAsync(id);
            if (user == null || user.Password != request.CurrentPassword) throw new WrongCredentialsException();

            user.Password = request.NewPassword;
            await _userRepository.UpdateWithSaveChangesAsync(user);
            return new GenericResponse();
        });

        public async Task<BaseResponse> SendTokenEmailAsync(string email)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.Get(x => x.Person!.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync() ?? throw new NotFoundUserException();

            var token = GeneratePasswordToken();
            await _tokenRepository.InsertWithSaveChangesAsync(new TokenControl(user.Id, token, true, DateTime.Now.Ticks, DateTime.Now.AddMinutes(30).Ticks));
            await _mailService.SendEmailAsync(new MailRequest
            {
                ToMail = email,
                Body = string.Format(Emails.ForgotPasswordEmail, user.Person!.Name, token),
                Subject = Emails.ForgotPasswordSubject
            });
            return new GenericResponse();
        });

        public async Task<BaseResponse> VerifyTokenAsync(VerifyTokenRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.Get(x => x.Person!.Email == request.Email.ToLower()).FirstOrDefaultAsync() ?? throw new NotFoundUserException();

            var token = await _tokenRepository.GetByToken(request.Token);
            if (token == null || token.IdUser != user.Id) throw new InvalidTokenException();

            token!.Valid = false;
            await _tokenRepository.UpdateWithSaveChangesAsync(token);
            return new GenericResponse();
        });

        public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.Get(x => x.Person!.Email.ToLower() == request.Email.ToLower()).FirstOrDefaultAsync() ?? throw new NotFoundUserException();

            return await UpdatePasswordAsync(user.Id, new UpdatePasswordRequest { CurrentPassword = user.Password, NewPassword = request.NewPassword });
        });
        #endregion

        #region DeleteUserAsync
        public async Task<BaseResponse> DeleteUserAsync(Guid id)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIDAsync(id) ?? throw new NotFoundUserException();

            await _userRepository.DeleteAsync(user.Id);
            await _personRepository.DeleteAsync(user.IdPerson);
            return new GenericResponse();
        });
        #endregion

        #region Private Methods
        private async Task InsertAsync(User item)
        {
            if (await _userRepository.AnyAsync(x => x.Login == item.Login)) throw new LoginException();
            if (await _personRepository.AnyAsync(x => x.CPF == item.Person!.CPF)) throw new CPFException();
            if (await _personRepository.AnyAsync(x => x.Email == item.Person!.Email)) throw new EmailException();

            await _userRepository.InsertWithSaveChangesAsync(item);
        }
        private static string GeneratePasswordToken()
        {
            Random random = new();
            int token = random.Next(0, 999999);
            return token.ToString("D6");
        }
        #endregion
    }
}
