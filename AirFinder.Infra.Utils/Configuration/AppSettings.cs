using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Utils.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public JwtSettings? Jwt { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class JwtSettings
    {
        public string? Secret { get; set; }
        public string? SessionExpirationHours { get; set; }
    }
}
