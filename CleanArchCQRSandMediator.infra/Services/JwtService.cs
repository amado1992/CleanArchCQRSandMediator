using CleanArchCQRSandMediator.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace CleanArchCQRSandMediator.infra.Services
{
    public class JwtService : IJwtService 
    {
        /// <summary>
        /// Jti token identifier
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetJtiFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
        }
    }
}
