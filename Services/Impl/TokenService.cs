using Microsoft.IdentityModel.Tokens;
using RestApiWithDontNet.Configurations;
using RestApiWithDontNet.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestApiWithDontNet.Services.Impl
{
    public class TokenService : ITokenService
    {
        private TokenConfiguration _configuration;

        public TokenService(TokenConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GeneratedAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration.Secret)
                );

            var signedCredentials = new SigningCredentials( secretKey, SecurityAlgorithms.HmacSha256 );
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                signingCredentials: signedCredentials,
                expires: DateTime.Now.AddMinutes(_configuration.Minutes)
            );
            return new JwtSecurityTokenHandler().WriteToken( tokenOptions );
        }

        public string GeneratedRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
           var tokenValidationsParameters = new TokenValidationParameters{
               ValidateAudience = false,
               ValidateIssuer = false,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(_configuration.Secret)
                ),
               ValidateLifetime = false
           };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationsParameters, out  securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
              !jwtSecurityToken.Header.Alg.
              Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture)
             ) throw new SecurityTokenException("Invalid Token.");
       
            return principal;
        }
    }
}
