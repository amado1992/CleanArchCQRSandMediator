using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Enums;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Plan
{
    /// <summary>
    /// Representa un pago o transacción financiera asociada a una suscripción.
    /// </summary>
    public class Payment : BaseEntity
    {
        /// <summary>Identificador único del pago.</summary>
        public int Id { get; set; }

        /// <summary>FK a la Suscripción que generó el pago.</summary>
        public int SubscriptionId { get; set; }

        /// <summary>Monto pagado.</summary>
        public decimal Amount { get; set; }

        /// <summary>Moneda del pago (Ej: USD, EUR).</summary>
        public string Currency { get; set; } = "USD";

        /// <summary>Estado del pago (Pendiente, Exitoso, Fallido, Reembolsado).</summary>
        public PaymentStatus Status { get; set; }

        /// <summary>ID de la transacción en la pasarela de pago externa.</summary>
        public string? ExternalPaymentId { get; set; }

        /// <summary>Fecha en que se realizó o confirmó el pago.</summary>
        public DateTimeOffset PaidAt { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>Navegación a la Suscripción.</summary>
        public Subscription Subscription { get; set; } = null!;
    }
}
