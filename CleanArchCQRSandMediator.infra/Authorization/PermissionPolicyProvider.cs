using CleanArchCQRSandMediator.Application.Authorization;
using CleanArchCQRSandMediator.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CleanArchCQRSandMediator.infra.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string PermissionPrefix = "Permission";
        private readonly IAuthorizationPolicyProvider _fallbackPolicyProvider;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // Fallback to default provider for non-permission policies
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // If the policy does not begin with "Permission", delegate to the default provider
            if (string.IsNullOrEmpty(policyName) || !policyName.StartsWith(PermissionPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return _fallbackPolicyProvider.GetPolicyAsync(policyName);
            }

            // Extract the resource and action portion of the policy name
            // Expected format: "Permission_Resource_Action" or "Permission_Action"
            var parts = policyName.Split('_', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return Task.FromResult<AuthorizationPolicy?>(null);
            }

            // The format can be "Permission_Resource_Action" or "Permission_Action"
            string? resource = null;
            string actionName;

            if (parts.Length == 3)
            {
                resource = parts[1];
                actionName = parts[2];
            }
            else if (parts.Length == 2)
            {
                actionName = parts[1];
            }
            else
            {
                return Task.FromResult<AuthorizationPolicy?>(null);
            }

            // Convert the action name to PermissionAction
            if (!Enum.TryParse<PermissionAction>(actionName, true, out var action))
            {
                return Task.FromResult<AuthorizationPolicy?>(null);
            }

            // Create the requirement and build the policy
            var requirement = new PermissionRequirement(action, resource);
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(requirement)
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }
}
