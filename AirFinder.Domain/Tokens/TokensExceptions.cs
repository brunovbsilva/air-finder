namespace AirFinder.Domain.Tokens
{
    public class InvalidTokenException : ArgumentException
    { public InvalidTokenException() : base("Invalid token") { } }
}
