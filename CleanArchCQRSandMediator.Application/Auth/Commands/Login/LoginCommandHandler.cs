using CleanArchCQRSandMediator.Application.Common.Configurations;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Auth;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;

        public LoginCommandHandler(ITokenService tokenService, JwtSettings jwtSettings, UserManager<ApplicationUser> userManager, IApplicationDbContext context, IJwtService jwtService)
        {
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Usuario desactivado");

            // Retrieve user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Obtain additional permissions (claims)
            var permissionClaims = await _userManager.GetClaimsAsync(user);

            // Generate access token
            var accessToken = _tokenService.GenerateAccessToken(user, roles, permissionClaims);

            // Generate and store refresh token
            var refreshToken = new Domain.Entities.Business.RefreshToken
            {
                Token = _tokenService.GenerateRefreshToken(),
                JwtId = _jwtService.GetJtiFromToken(accessToken),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                ApplicationUserId = user.Id
            };

            // We remove expired tokens
            _context.RefreshTokens.RemoveRange(user.RefreshTokens.Where(rt => rt.ExpiresAt < DateTime.UtcNow));
            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            // Update last login
            // user.LastLoginAt = DateTime.UtcNow;

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            };
        }
    }
}
