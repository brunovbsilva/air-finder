namespace AirFinder.Domain.Tokens
{
    public class InvalidTokenException : Exception
    { public InvalidTokenException() : base("Invalid token") { } }
}
