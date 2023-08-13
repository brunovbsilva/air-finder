namespace AirFinder.Domain.Battlegrounds
{
    public class NotFoundBattlegroundException : ArgumentException
    { public NotFoundBattlegroundException() : base("Battleground not found") { } }
}
