namespace CleanArchCQRSandMediator.Application.Dtos.Brands
{
    public class BrandListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
    }
}
