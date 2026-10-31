using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.Domain.Enums;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Plan
{
    /// <summary>
    /// Representa un plan comercial en el catálogo (Ej: Free, Pro, Enterprise).
    /// </summary>
    public class Plan : BaseEntity
    {
        /// <summary>Identificador único del plan.</summary>
        public int Id { get; set; }

        /// <summary>Nombre comercial del plan (Ej: "Plan Pro").</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Descripción de los beneficios del plan para mostrar en el frontend.</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>Precio base del plan.</summary>
        public decimal Price { get; set; }

        /// <summary>Frecuencia de cobro (Mensual, Anual, etc.).</summary>
        public BillingPeriod BillingPeriod { get; set; }

        /// <summary>Indica si el plan está disponible para la venta.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// User who created the plan. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the plan.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }

        // Navigation Properties
        public List<Feature> Features { get; } = [];
        public List<PlanFeature> PlanFeatures { get; } = [];
        public List<Subscription> Subscriptions { get; } = [];
    }
}
