using System.Security.Claims;

namespace RestApiWithDontNet.Services.Contracts
{
    public interface ITokenService
    {
        string GeneratedAccessToken(IEnumerable<Claim> claims);
        string GeneratedRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
