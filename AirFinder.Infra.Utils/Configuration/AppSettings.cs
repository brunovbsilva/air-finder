namespace AirFinder.Infra.Utils.Configuration
{
    public class AppSettings
    {
        public JwtSettings? Jwt { get; set; }
    }

    public class JwtSettings
    {
        public string? Secret { get; set; }
        public string? SessionExpirationHours { get; set; }
    }
}
