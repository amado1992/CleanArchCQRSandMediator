using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Enums;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Provider : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CellPhone { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
    }
}
