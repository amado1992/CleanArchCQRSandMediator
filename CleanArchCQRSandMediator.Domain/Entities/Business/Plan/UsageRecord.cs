using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Plan
{
    /// <summary>
    /// Registra el consumo real de una característica de tipo Quota (Ej: cuántos proyectos ha creado).
    /// </summary>
    public class UsageRecord : BaseEntity
    {
        /// <summary>Identificador único del registro de uso.</summary>
        public int Id { get; set; }

        /// <summary>FK a la Suscripción bajo la cual se consume el recurso.</summary>
        public int SubscriptionId { get; set; }

        /// <summary>FK a la Feature que se está consumiendo.</summary>
        public int FeatureId { get; set; }

        /// <summary>Cantidad consumida en esta acción (normalmente 1).</summary>
        public int Quantity { get; set; } = 1;

        /// <summary>Fecha y hora exacta del consumo.</summary>
        public DateTimeOffset RecordedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation Properties
        public Subscription Subscription { get; set; } = null!;
        public Feature Feature { get; set; } = null!;

        /// <summary>
        /// User who created the feature. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the feature.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
