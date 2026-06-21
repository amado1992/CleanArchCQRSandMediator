using CleanArchCQRSandMediator.Application.Common.Configurations;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Auth;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;


        public RefreshTokenCommandHandler(ITokenService tokenService, JwtSettings jwtSettings, UserManager<ApplicationUser> userManager, IApplicationDbContext context, IJwtService jwtService)
        {
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new SecurityTokenException("Id usuario es nulo");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new SecurityTokenException("Usuario no encontrado");

            var storedRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.ApplicationUserId.ToString() == userId && !rt.IsRevoked, cancellationToken);

            if (storedRefreshToken == null || storedRefreshToken.ExpiresAt < DateTime.UtcNow)
                throw new SecurityTokenException("Refresh token inválido o expirado");

            // Rotación: revocar el refresh token usado
            storedRefreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedRefreshToken);

            // Generar nuevo access token
            var roles = await _userManager.GetRolesAsync(user);
            var permissionClaims = await _userManager.GetClaimsAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles, permissionClaims);

            // Generar nuevo refresh token
            var newRefreshToken = new Domain.Entities.Business.RefreshToken
            {
                Token = _tokenService.GenerateRefreshToken(),
                JwtId = _jwtService.GetJtiFromToken(newAccessToken),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                ApplicationUserId = user.Id
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            };
        }
    }
}
