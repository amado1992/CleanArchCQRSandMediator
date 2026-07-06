using CleanArchCQRSandMediator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchCQRSandMediator.Application.Authorization
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(PermissionAction action, string? resource = null)
        {
            // Create a unique policy name: "Permission_Resource_Action"
            var resourcePart = string.IsNullOrEmpty(resource) ? "" : $"_{resource}";
            Policy = $"Permission{resourcePart}_{action}";
        }
    }
}
