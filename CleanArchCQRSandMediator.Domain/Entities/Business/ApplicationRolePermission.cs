using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class ApplicationRolePermission
    {
        public int Id { get; set; }
        public int ApplicationRoleId { get; set; }
        public ApplicationRole ApplicationRole { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
