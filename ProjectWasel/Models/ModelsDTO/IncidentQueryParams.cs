namespace ProjectWasel.Models.ModelsDTO
{
    /// <summary>
    /// Query parameters for GET /incident — supports filtering, sorting, and pagination.
    /// All parameters are optional; sensible defaults are applied.
    /// </summary>
    public class IncidentQueryParams
    {
        // ───────────── Filters ─────────────

        /// <summary>Filter by incident type: closure | delay | accident | weather hazard | other</summary>
        public string? Type { get; set; }

        /// <summary>Filter by severity: Low | Medium | High | Critical</summary>
        public string? Severity { get; set; }

        /// <summary>Filter by status: active | verified | closed</summary>
        public string? Status { get; set; }

        /// <summary>Filter incidents linked to a specific checkpoint.</summary>
        public int? CheckpointId { get; set; }

        /// <summary>Return only incidents created ON or AFTER this UTC date.</summary>
        public DateTime? CreatedAfter { get; set; }

        /// <summary>Return only incidents created ON or BEFORE this UTC date.</summary>
        public DateTime? CreatedBefore { get; set; }

        // ───────────── Sorting ─────────────

        /// <summary>
        /// Field to sort by. Allowed values (case-insensitive):
        /// createdAt (default) | updatedAt | severity | type | status
        /// </summary>
        public string SortBy { get; set; } = "createdAt";

        /// <summary>Sort direction: asc | desc (default).</summary>
        public string SortOrder { get; set; } = "desc";

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
