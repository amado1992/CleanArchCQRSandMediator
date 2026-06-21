using CleanArchCQRSandMediator.Domain.Common;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Purchase : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime DatePurchase { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;
    }
}
