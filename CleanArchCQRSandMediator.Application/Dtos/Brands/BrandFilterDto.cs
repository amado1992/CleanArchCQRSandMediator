namespace CleanArchCQRSandMediator.Application.Dtos.Brands
{
    public class BrandFilterDto
    {
        // Textual search (Name and Description)
        public string? Search { get; set; }

        // Filter by TenantId
        public int? TenantId { get; set; }

        // Sorting: "name", "createdAt" (default "name")
        public string SortBy { get; set; } = "name";
        public string SortOrder { get; set; } = "asc"; // "asc" o "desc"

        // Pagination
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int Page { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 10 : value);
        }
    }
}
