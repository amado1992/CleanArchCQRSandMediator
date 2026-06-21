using CleanArchCQRSandMediator.Domain.Common;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Currency : BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public List<Purchase> Purchases { get; } = [];
    }
}
