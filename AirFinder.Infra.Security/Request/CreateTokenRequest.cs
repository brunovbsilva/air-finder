namespace AirFinder.Infra.Security.Request
{
    public class CreateTokenRequest
    {
        public string Login { get; set; } = String.Empty;
        public Guid UserId { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<string> Scopes { get; set; } = new List<string>();
    }
}
