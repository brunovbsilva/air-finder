using AirFinder.Infra.Utils.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirFinder.API.HealthCheck
{
    public static class HealthCheckBuilder
    {
        public static IHealthChecksBuilder AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = Builders.BuildConnectionString(configuration);
            var builder = services.AddHealthChecks().AddSqlServer(conn, name: "database");
            return builder;
        }
    }
}
