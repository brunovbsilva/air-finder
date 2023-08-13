namespace AirFinder.Domain.Battlegrounds.Models.Requests
{
    public class CreateBattlegroundRequest
    {
        public string Name { get; set; } = String.Empty;
        public string ImageBase64 { get; set; } = String.Empty;
        public string CEP { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public int Number { get; set; } = 0;
        public string City { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
    }
}
