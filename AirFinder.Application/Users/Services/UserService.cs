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

namespace AirFinder.Application.Users.Services
{
    public class UserService : BaseService, IUserService
    {
        readonly IUserRepository _userRepository;
        readonly IPersonRepository _personRepository;
        readonly ITokenRepository _tokenRepository;
        readonly IJwtService _jwtService;
        public UserService(IUserRepository userRepository,
            IPersonRepository personRepository,
            ITokenRepository tokenRepository,
            IJwtService jwtService,
            INotification notification,
            IMailService mailService) : base(notification, mailService)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _tokenRepository = tokenRepository;
            _jwtService = jwtService;
        }

        #region Helpers
        private async Task InsertAsync(User item)
        {
            if (await _userRepository.GetByLoginAsync(item.Login) != null) throw new LoginException();
            if (await _personRepository.GetByCPFAsync(item.Person!.CPF) != null) throw new CPFException();
            if (await _personRepository.GetByEmailAsync(item.Person.Email) != null) throw new EmailException();

            await _userRepository.InsertWithSaveChangesAsync(item);
        }
        #endregion
        
        #region CreateUserAsync
        public async Task<CreateUserResponse?> CreateUserAsync(UserRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = new User
            {
                Login = request.Login.ToLower(),
                Password = request.Password,
                Roll = UserRoll.Default,
                Person = new Person(
                    request.Name,
                    request.Email.ToLower(),
                    request.Birthday,
                    request.CPF,
                    request.Gender,
                    request.Phone
                )
            };
            await InsertAsync(user);
            return (CreateUserResponse)user;
        });
        #endregion

        #region CreateAnotherUserAsync
        public async Task<BaseResponse?> CreateAnotherUserAsync(CreateAnotherUserRequest request, Guid userId)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundUserException();
            var newUser = new User
            {
                Login = request.Login.ToLower(),
                Password = request.Password,
                Roll = request.Role < user.Roll ? user.Roll : request.Role,
                IdPerson = user.IdPerson,
                Person = null
            };
            await _userRepository.InsertWithSaveChangesAsync(newUser);
            return new GenericResponse();
        });
        #endregion

        #region Login
        public async Task<LoginResponse?> LoginAsync(string login, string password)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByLoginAsync(login.ToLower());
            if (user == null || user.Password != password) throw new WrongCredentialsException();

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

        #region Update Password
        public async Task<BaseResponse?> UpdatePasswordAsync(Guid id, UpdatePasswordRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIdAsync(id) ?? throw new NotFoundUserException();
            if (user.Password != request.CurrentPassword) throw new WrongCredentialsException();

            user.Password = request.NewPassword;
            await _userRepository.UpdateWithSaveChangesAsync(user);
            return new GenericResponse();
        });

        public async Task<BaseResponse?> SendTokenEmailAsync(string email)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByEmailAsync(email.ToLower()) ?? throw new NotFoundUserException();

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

        public async Task<BaseResponse?> VerifyTokenAsync(VerifyTokenRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.ToLower()) ?? throw new NotFoundUserException();

            var token = await _tokenRepository.GetByToken(request.Token);

            if (token == null || token.IdUser != user.Id) throw new InvalidTokenException();

            token.Valid = false;
            await _tokenRepository.UpdateWithSaveChangesAsync(token);
            return new GenericResponse();
        });

        public async Task<BaseResponse?> ChangePasswordAsync(ChangePasswordRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.ToLower()) ?? throw new NotFoundUserException();

            return await UpdatePasswordAsync(user.Id, new UpdatePasswordRequest { CurrentPassword = user.Password, NewPassword = request.NewPassword });
        });
        #endregion
        
        #region Delete
        public async Task<BaseResponse?> DeleteUserAsync(Guid id)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIdAsync(id) ?? throw new NotFoundUserException();

            await _userRepository.DeleteAsync(user.Id);
            await _personRepository.DeleteAsync(user.IdPerson);
            return new GenericResponse();
        });
        #endregion
        
        #region Private Methods
        private static string GeneratePasswordToken()
        {
            Random random = new();
            int token = random.Next(0, 999999);
            return token.ToString("D6");
        }
        #endregion
    }
}
