namespace AirFinder.Domain.Games
{
    public class NotFoundGameException : ArgumentException
    { public NotFoundGameException() : base("Game not found") { } }
}
