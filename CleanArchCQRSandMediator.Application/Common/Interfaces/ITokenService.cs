using CleanArchCQRSandMediator.Domain.Entities.Identity;
using System.Security.Claims;

namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface ITokenService
    {
        public string GenerateAccessToken(ApplicationUser user, IList<string> roles, IList<Claim>? additionalClaims = null);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
