using FlexiTime_Backend.Infra.Mongo;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FlexiTime_Backend.Services.Token
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, bool rememberMe);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateTokenWithMultipleClaims(IEnumerable<Claim> claims, double expiration = 3600);
        SecurityToken ValidateToken(string token);
    }
}
