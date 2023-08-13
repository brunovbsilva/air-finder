namespace AirFinder.Domain.GameLogs
{
    public class NotFoundGameLogException : Exception
    { public NotFoundGameLogException() : base("Log not found") { } }
}
