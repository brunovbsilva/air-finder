namespace AirFinder.Domain.Users
{
    public class NotFoundUserException : ArgumentException
    { public NotFoundUserException() : base("User not found") { } }
}
