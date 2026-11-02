using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Plan
{
    /// <summary>
    /// Tabla puente que relaciona un Plan con sus Features, añadiendo los límites específicos.
    /// </summary>
    public class PlanFeature
    {
        // public int Id { get; set; }

        /// <summary>FK al Plan.</summary>
        public int PlanId { get; set; }

        /// <summary>FK a la Feature.</summary>
        public int FeatureId { get; set; }

        /// <summary>
        /// Límite de uso de la característica.
        /// Si es Boolean, debe ser NULL y el sistema se fija en IsEnabled para saber si es true o false.
        /// Si es Quota, será un número >= 0, o -1 para representar "Ilimitado".
        /// </summary>
        public int? LimitValue { get; set; }

        /// <summary>Permite habilitar o deshabilitar esta feature en el plan sin borrar el registro.</summary>
        public bool IsEnabled { get; set; } = true;

        // Navigation Properties
        public Plan Plan { get; set; } = null!;
        public Feature Feature { get; set; } = null!;

        /// <summary>
        /// User who created the plan feature. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the plan feature.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
