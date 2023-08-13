namespace AirFinder.Domain.BattleGrounds
{
    public class NotFoundBattlegroundException : Exception
    { public NotFoundBattlegroundException() : base("Battleground not found") { } }
}
