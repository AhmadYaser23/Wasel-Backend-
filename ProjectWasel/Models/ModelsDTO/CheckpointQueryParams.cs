namespace ProjectWasel.Models.ModelsDTO
{
    /// <summary>
    /// Query parameters for GET /checkpoint — supports filtering, sorting, and pagination.
    /// </summary>
    public class CheckpointQueryParams
    {
        // ───────────── Filters ─────────────

        /// <summary>Filter by status: active | closed | delayed</summary>
        public string? Status { get; set; }

        /// <summary>Partial name search (case-insensitive contains).</summary>
        public string? Name { get; set; }

        // ───────────── Sorting ─────────────

        /// <summary>
        /// Field to sort by. Allowed values (case-insensitive):
        /// name (default) | status | lastUpdated
        /// </summary>
        public string SortBy { get; set; } = "name";

        /// <summary>Sort direction: asc (default) | desc.</summary>
        public string SortOrder { get; set; } = "asc";

        // ───────────── Pagination ─────────────

        private int _page = 1;
        /// <summary>Page number, 1-based. Defaults to 1.</summary>
        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        private int _pageSize = 10;
        /// <summary>Records per page. Min 1, Max 50. Defaults to 10.</summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 1 : value > 50 ? 50 : value;
        }
    }
}
