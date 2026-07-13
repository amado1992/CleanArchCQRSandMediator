using CleanArchCQRSandMediator.Application.Common.Configurations;
using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Auth;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                throw new NotFoundException($"User ID Claim {userIdClaim} not found");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), userId);

            var jwtId = _jwtService.GetJtiFromToken(request.AccessToken);
            var storedRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken
                                        && rt.ApplicationUserId == userId
                                        && rt.JwtId == jwtId
                                        && !rt.IsRevoked, cancellationToken);

            if (storedRefreshToken == null || storedRefreshToken.ExpiresAt < DateTime.UtcNow)
                throw new SecurityTokenException("Refresh token invalid or expired");

            // Rotation: revoke the used refresh token
            storedRefreshToken.IsRevoked = true;

            // Generate new access token
            var roles = await _userManager.GetRolesAsync(user);
            var permissionClaims = await _userManager.GetClaimsAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles, permissionClaims);

            // Generate new refresh token
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
