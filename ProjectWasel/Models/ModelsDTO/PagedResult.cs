namespace ProjectWasel.Models.ModelsDTO
{
    /// <summary>
    /// Generic wrapper returned from any paginated API endpoint.
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>The records for the current page.</summary>
        public List<T> Data { get; set; } = new();

        /// <summary>Total number of records matching the current filters (before paging).</summary>
        public int TotalCount { get; set; }

        /// <summary>Current page number (1-based).</summary>
        public int Page { get; set; }

        /// <summary>Number of records per page.</summary>
        public int PageSize { get; set; }

        /// <summary>Total number of pages.</summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

        /// <summary>Whether a previous page exists.</summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>Whether a next page exists.</summary>
        public bool HasNextPage => Page < TotalPages;
    }
}
