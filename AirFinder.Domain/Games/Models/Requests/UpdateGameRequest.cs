namespace AirFinder.Domain.Games.Models.Requests
{
    public class UpdateGameRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long Date { get; set; }
    }
}
