using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Purchase : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Date of purchase (according to the invoice/supplier document).
        /// No time, no time zone.
        /// </summary>
        public DateOnly DatePurchase { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;
        public string Observation { get; set; } = string.Empty;
        public int BranchOfficeId { get; set; }
        public BranchOffice BranchOffice { get; set; } = null!;
        public int ProviderId { get; set; }
        public Provider Provider { get; set; } = null!;
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        /// <summary>
        /// User who created the purchase. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the purchase.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
