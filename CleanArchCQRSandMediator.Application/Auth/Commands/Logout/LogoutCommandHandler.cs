using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtService _jwtService;

        public LogoutCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IJwtService jwtService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _jwtService = jwtService;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the userId from the token (already authenticated)
            var userId = _currentUserService.GetUserId();

            // 2. Obtain the access token from the Authorization header.
            var accessToken = _currentUserService.GetAccessToken();
            if (string.IsNullOrEmpty(accessToken))
                return;

            // 3. Extract the jti from the access token
            var jwtId = _jwtService.GetJtiFromToken(accessToken);
            if (string.IsNullOrEmpty(jwtId))
                return;

            // 4. Look up the refresh token associated with the user and the access token's JTI.
            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.ApplicationUserId == userId
                                        && rt.JwtId == jwtId
                                        && !rt.IsRevoked, cancellationToken);

            // 5. If it doesn't exist, we do nothing (successful logout)
            if (refreshTokenEntity == null)
                return;

            refreshTokenEntity.IsRevoked = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
