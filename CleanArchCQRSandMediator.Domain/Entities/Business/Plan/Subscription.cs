using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.Domain.Enums;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Plan
{
    /// <summary>
    /// Representa el contrato de un usuario con un plan específico en un período de tiempo.
    /// </summary>
    public class Subscription : BaseEntity
    {
        /// <summary>Identificador único de la suscripción.</summary>
        public int Id { get; set; }

        /// <summary>FK lógica al usuario de Identity (AspNetUsers.Id).</summary>
        public int UserId { get; set; }

        /// <summary>FK al Plan contratado.</summary>
        public int PlanId { get; set; }

        /// <summary>Estado actual de la suscripción (Activa, Vencida, Cancelada, etc.).</summary>
        public SubscriptionStatus Status { get; set; }

        /// <summary>Fecha en que inició la suscripción.</summary>
        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>Fecha de finalización (NULL si es activa o ilimitada).</summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>Fecha en que el usuario canceló explícitamente la suscripción.</summary>
        public DateTimeOffset? CancelledAt { get; set; }

        /// <summary>ID de la suscripción en la pasarela de pago externa (Stripe, PayPal, etc.).</summary>
        public string? ExternalSubscriptionId { get; set; }

        /// <summary>
        /// User who created the subscription. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the subscription.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }

        // Navigation Properties
        public Plan Plan { get; set; } = null!;
        public List<Payment> Payments { get; } = [];
        public List<UsageRecord> UsageRecords { get; } = [];
    }
}
