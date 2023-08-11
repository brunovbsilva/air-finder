namespace AirFinder.Domain.BattleGrounds
{
    public class NotFoundBattlegroundException : ArgumentException
    { public NotFoundBattlegroundException() : base("Battleground not found") { } }
}
