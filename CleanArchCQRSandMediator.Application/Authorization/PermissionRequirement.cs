using CleanArchCQRSandMediator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchCQRSandMediator.Application.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionAction Action { get; }
        public string Resource { get; } // Optional, for permissions per resource (e.g. "articles", "users")

        public PermissionRequirement(PermissionAction action, string? resource = null)
        {
            Action = action;
            Resource = resource ?? string.Empty;
        }

        // Method to generate the permission name in "resource.action" format
        public string GetPermissionName() => string.IsNullOrEmpty(Resource)
            ? Action.ToString().ToLower()
            : $"{Resource}.{Action.ToString().ToLower()}";
    }
}
