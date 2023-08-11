namespace AirFinder.Domain.GameLogs
{
    public class NotFoundGameLogException : ArgumentException
    { public NotFoundGameLogException() : base("Log not found") { } }
}
