using CleanArchCQRSandMediator.Domain.Common;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class PaymentMethod : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Purchase> Purchases { get; } = [];
    }
}
