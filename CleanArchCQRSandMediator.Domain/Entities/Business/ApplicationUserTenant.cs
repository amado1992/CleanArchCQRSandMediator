using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class ApplicationUserTenant
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public int TenantId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
    }
}
