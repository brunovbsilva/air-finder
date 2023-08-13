using SendGrid.Helpers.Errors.Model;

namespace AirFinder.Domain.Users
{
    public class NotFoundUserException : ArgumentException
    { public NotFoundUserException() : base("User not found") { } }

    public class LoginException : MethodNotAllowedException
    { public LoginException() : base("Login already registered") { } }

    public class WrongCredentialsException : ArgumentException
    { public WrongCredentialsException() : base("Wrong credentials") { } }
}
