namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    /// <summary>
    /// Resource
    /// </summary>
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Permission> Permissions { get; } = [];
    }
}
