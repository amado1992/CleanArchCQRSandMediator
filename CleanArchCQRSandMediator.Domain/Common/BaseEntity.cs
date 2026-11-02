namespace CleanArchCQRSandMediator.Domain.Common
{
    public abstract class BaseEntity
    {
        /// <summary>Fecha de creación en el sistema.</summary>
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>Fecha de actualización en el sistema.</summary>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
