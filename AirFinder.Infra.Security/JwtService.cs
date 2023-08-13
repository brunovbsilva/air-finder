using AirFinder.Infra.Security.Constants;
using AirFinder.Infra.Security.Request;
using AirFinder.Infra.Utils.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AirFinder.Infra.Security
{
    public class JwtService : IJwtService
    {
        private readonly AppSettings _appSettings;
        public JwtService(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string CreateToken(CreateTokenRequest request)
        {
            var secret = _appSettings.Jwt?.Secret;
            var timeHoursExpiration = Convert.ToInt32(_appSettings.Jwt?.SessionExpirationHours);

            var signinKey = new SigningCredentials(
                new SymmetricSecurityKey(Convert.FromBase64String(secret ?? String.Empty)),
                SecurityAlgorithms.HmacSha256
            );

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = GetClaims(request),
                Expires = DateTime.UtcNow.AddHours(timeHoursExpiration),
                SigningCredentials = signinKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);

            return tokenHandler.WriteToken(token);
        }

        private static ClaimsIdentity GetClaims(CreateTokenRequest request)
        {
            var identity = new ClaimsIdentity("JWT");

            identity.AddClaim(new Claim(JwtClaims.CLAIM_LOGIN, request.Login.ToString()));
            identity.AddClaim(new Claim(JwtClaims.CLAIM_USER_ID, request.UserId.ToString()));
            identity.AddClaim(new Claim(JwtClaims.CLAIM_USER_NAME, request.Name.ToString()));
            request.Scopes?.ForEach(scope => identity.AddClaim(new Claim(JwtClaims.CLAIM_SCOPES, scope)));
           
            return identity;
        }
    }
}
