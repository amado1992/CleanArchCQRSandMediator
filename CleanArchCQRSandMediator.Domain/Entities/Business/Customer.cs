using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.Domain.Enums;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Customer : BaseEntity
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

        /// <summary>
        /// User who created the customer. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the customer.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
