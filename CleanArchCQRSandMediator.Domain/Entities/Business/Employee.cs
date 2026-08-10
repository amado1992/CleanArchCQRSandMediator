using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Employee : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string Email { get; set; } = string.Empty;
        public string CellPhone { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        /// <summary>
        /// User creator 
        /// </summary>
        public int ApplicationUserCreatorId { get; set; }
        public virtual ApplicationUser ApplicationUserCreator { get; set; } = null!;
        /// <summary>
        /// User updater
        /// </summary>
        public int? ApplicationUserUpdaterId { get; set; }
        public virtual ApplicationUser? ApplicationUserUpdater { get; set; } = null!;
    }
}
