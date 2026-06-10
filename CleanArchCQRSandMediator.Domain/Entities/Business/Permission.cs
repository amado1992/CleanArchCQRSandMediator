using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Permission
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;
        public int ActionId { get; set; }
        public Action Action { get; set; } = null!;
        public List<ApplicationRole> ApplicationRoles { get; } = [];
        public List<ApplicationRolePermission> ApplicationRolePermissions { get; } = [];
    }
}
