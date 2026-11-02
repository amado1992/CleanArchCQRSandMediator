using CleanArchCQRSandMediator.Domain.Common;

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

        /// <summary>Navegación a la Suscripción.</summary>
        public Subscription Subscription { get; set; } = null!;

        /// <summary>Navegación a la Feature.</summary>
        public Feature Feature { get; set; } = null!;
    }
}
