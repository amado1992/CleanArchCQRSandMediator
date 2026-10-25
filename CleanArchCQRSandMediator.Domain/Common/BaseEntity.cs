namespace CleanArchCQRSandMediator.Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
