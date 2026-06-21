using CleanArchCQRSandMediator.Domain.Common;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class BranchOffice : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CellPhone { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
    }
}
