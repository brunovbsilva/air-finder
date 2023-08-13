namespace AirFinder.Domain.Games
{
    public class NotFoundGameException : Exception
    { public NotFoundGameException() : base("Game not found") { } }
}
