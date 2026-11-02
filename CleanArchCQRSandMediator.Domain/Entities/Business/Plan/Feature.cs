using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.Domain.Enums;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Plan
{
    /// <summary>
    /// Representa una capacidad o funcionalidad específica del sistema (Ej: Exportar PDF, Límite de Proyectos).
    /// </summary>
    public class Feature : BaseEntity
    {
        /// <summary>Identificador único de la característica.</summary>
        public int Id { get; set; }

        /// <summary>Código único e inmutable usado en el código (Policies, Guards). Ej: "export_pdf".</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Nombre legible para el usuario (Ej: "Exportación a PDF").</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Descripción de qué hace esta característica.</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>Tipo de característica: Boolean (Sí/No) o Quota (Límite numérico).</summary>
        public FeatureType Type { get; set; }

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

        /// <summary>
        /// Navigation Properties de una Many-to-many relationships (PlanFeature)
        /// </summary>
        public List<Plan> Plans { get; } = [];
        public List<PlanFeature> PlanFeatures { get; } = [];

        /// <summary>
        /// Navigation Properties de una Many-to-many relationships (UsageRecord)
        /// </summary>
        public List<Subscription> Subscriptions { get; } = [];
        public List<UsageRecord> UsageRecords { get; } = [];
    }
}
