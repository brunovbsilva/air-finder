using AirFinder.Application.Common;
using AirFinder.Application.Email.Models.Request;
using AirFinder.Application.Email.Services;
using AirFinder.Application.Users.Models.Request;
using AirFinder.Application.Users.Models.Response;
using AirFinder.Domain.People;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Tokens;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;

namespace AirFinder.Application.Users.Services
{
    public class UserService : BaseService, IUserService
    {
        readonly IUserRepository _userRepository;
        readonly IPersonRepository _personRepository;
        readonly ITokenRepository _tokenRepository;
        public UserService(IUserRepository userRepository,
            IPersonRepository personRepository,
            ITokenRepository tokenRepository,
            INotification notification,
            IMailService mailService) : base(notification, mailService)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _tokenRepository = tokenRepository;
        }

        #region Helpers
        public async Task Insert(User item)
        {
            if (await _userRepository.GetByLoginAsync(item.Login) != null) throw new ArgumentException(nameof(item.Login) + " already exists.");
            if (await _personRepository.GetByCPFAsync(item.Person.CPF) != null) throw new ArgumentException(nameof(item.Person.CPF) + " already exists.");
            if (await _personRepository.GetByEmailAsync(item.Person.Email) != null) throw new ArgumentException(nameof(item.Person.Email) + " already exists.");

            await _userRepository.InsertWithSaveChangesAsync(item);
        }
        #endregion
        
        #region CreateUserAsync
        public async Task<CreateUserResponse?> CreateUserAsync(UserRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = new User
            {
                Login = request.Login,
                Password = request.Password,
                Roll = UserRoll.Default,
                Person = new Person(
                    request.Name,
                    request.Email,
                    request.Birthday,
                    request.CPF,
                    request.Gender,
                    request.Phone
                )
            };
            await Insert(user);
            return (CreateUserResponse)user;
        });
        #endregion

        #region Login
        public async Task<LoginResponse?> Login(string login, string password)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null || user.Password != password) throw new ArgumentException("Wrong credentials.");
            return new LoginResponse
            {
                User = user
            };
        });
        #endregion

        #region Update Password
        public async Task<BaseResponse?> UpdatePasswordAsync(int id, UpdatePasswordRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new ArgumentException("User not found.");
            if (user.Password != request.CurrentPassword) throw new ArgumentException("Incorrect password.");

            user.Password = request.NewPassword;
            await _userRepository.UpdateWithSaveChangesAsync(user);
            return new GenericResponse();
        });

        public async Task<BaseResponse?> SendTokenEmail(string email)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new ArgumentException("User not found.");

            var token = GeneratePasswordToken();
            await _tokenRepository.InsertWithSaveChangesAsync(new TokenControl(user.Id, token, true, DateTime.Now, DateTime.Now.AddMinutes(30)));
            await _mailService.SendEmailAsync(new MailRequest
            {
                ToMail = email,
                Body = ForgotPasswordEmailBody(user.Person.Name, token),
                Subject = "Solicitação de token para alteração de senha"
            });
            return new GenericResponse();
        });

        public async Task<BaseResponse?> VerifyToken(VerifyTokenRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) throw new ArgumentException(String.Concat("User with email", nameof(request.Email), " not found"));

            var _token = await _tokenRepository.GetByToken(request.Token);
            if (_token == null || _token.IdUser != user.Id) throw new ArgumentException("Token inválido");

            _token.Valid = false;
            await _tokenRepository.UpdateWithSaveChangesAsync(_token);
            return new GenericResponse();
        });

        public async Task<BaseResponse?> ChangePassword(ChangePasswordRequest request)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if(user == null) throw new ArgumentException("User not found.");

            return await UpdatePasswordAsync(user.Id, new UpdatePasswordRequest { CurrentPassword = user.Password, NewPassword = request.NewPassword });
        });
        #endregion
        
        #region Delete
        public async Task<BaseResponse?> Delete(int id)
        => await ExecuteAsync(async () =>
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new ArgumentException("User not found.");

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

        private static string ForgotPasswordEmailBody(string name, string token)
        {
            return string.Format(@"<!DOCTYPE html>
                <html>
                <head>
	                <meta charset=""UTF-8"">
	                <title>Seu token para alteração de senha</title>
                </head>
                <body>
	                <h1>Seu token para alteração de senha</h1>
	                <p>Prezado(a) {0},</p>
	                <p>Recebemos sua solicitação de alteração de senha para sua conta. Para prosseguir com a alteração de senha, utilize o seguinte token de 6 dígitos:</p>
	                <p style=""font-size: 24px; font-weight: bold;"">{1}</p>
	                <p>Este token é válido por 30 minutos a partir do recebimento deste e-mail. Por favor, não compartilhe este token com ninguém, pois é exclusivo para você. Após a expiração do prazo de 24 horas, o token não poderá ser utilizado e você precisará solicitar um novo token de alteração de senha.</p>
	                <p>Se você não solicitou esta alteração de senha, por favor, ignore este e-mail e entre em contato conosco imediatamente para proteger sua conta.</p>
	                <p>Se tiver alguma dúvida ou preocupação, por favor, não hesite em nos contatar.</p>
	                <br>
	                <p>Atenciosamente,</p>
	                <p>Air Finder</p>
                </body>
                </html>", name, token);
        }
        #endregion
    }
}
