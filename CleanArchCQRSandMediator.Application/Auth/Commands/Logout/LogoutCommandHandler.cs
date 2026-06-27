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
            // The handler obtains the userId from the current user service
            var userId = _currentUserService.GetUserId();

            var jwtId = _jwtService.GetJtiFromToken(request.AccessToken);

            // Find the refresh token that matches the token and the userId
            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken 
                                        && rt.ApplicationUserId == userId
                                        && rt.JwtId == jwtId, cancellationToken);

            if (refreshTokenEntity != null)
            {
                refreshTokenEntity.IsRevoked = true;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
