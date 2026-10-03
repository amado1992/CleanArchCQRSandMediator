namespace CleanArchCQRSandMediator.Application.Dtos.Organizations
{
    public class OrganizationFilterDto
    {
        // Textual search (Name, Description, Slug)
        public string? Search { get; set; }

        // Filter by active/inactive status
        public bool? IsActive { get; set; }

        // Filter by exact slug (useful for lookups)
        public string? Slug { get; set; }

        // Sorting: "name", "slug", "createdAt" (default "name")
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
