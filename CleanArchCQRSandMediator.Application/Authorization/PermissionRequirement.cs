using CleanArchCQRSandMediator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchCQRSandMediator.Application.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionAction Action { get; }
        public string Resource { get; } // Opcional, para permisos por recurso (ej. "articles", "users")

        public PermissionRequirement(PermissionAction action, string? resource = null)
        {
            Action = action;
            Resource = resource ?? string.Empty;
        }

        // Método para generar el nombre del permiso en formato "resource.action"
        public string GetPermissionName() => string.IsNullOrEmpty(Resource)
            ? Action.ToString().ToLower()
            : $"{Resource}.{Action.ToString().ToLower()}";
    }
}
