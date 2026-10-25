using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Currency : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public List<Purchase> Purchases { get; } = [];

        /// <summary>
        /// User who created the currency. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the currency.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
