using CleanArchCQRSandMediator.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchCQRSandMediator.infra.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Unauthenticated user or invalid ID.");
            return userId;
        }

        public string? GetUserEmail()
            => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated()
            => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? GetFullName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue("fullName");
        }
    }
}
