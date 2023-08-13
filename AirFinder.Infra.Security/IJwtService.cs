using AirFinder.Infra.Security.Request;

namespace AirFinder.Infra.Security
{
    public interface IJwtService
    {
        string CreateToken(CreateTokenRequest request);
    }
}
