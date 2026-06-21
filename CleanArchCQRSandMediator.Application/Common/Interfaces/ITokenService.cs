using CleanArchCQRSandMediator.Domain.Entities.Identity;
using System.Security.Claims;

namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles, IList<Claim>? additionalClaims = null);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
