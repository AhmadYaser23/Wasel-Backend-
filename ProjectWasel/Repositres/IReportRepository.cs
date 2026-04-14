// Models/Repositres/IReportRepository.cs
namespace ProjectWasel.Models.Repositres
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync(
            int page, int pageSize,
            string? category = null,
            string? status = null,
            string? sortBy = null);

        Task<Report?> GetByIdAsync(int id);
        Task<Report> CreateAsync(Report report);
        Task<Report?> UpdateAsync(Report report);
        Task<bool> DeleteAsync(int id);

        Task<Report?> FindDuplicateAsync(double lat, double lng,
            string category, int radiusMeters = 500);

        Task<bool> HasUserVotedAsync(int reportId, int userId);
        Task AddVoteAsync(ReportVote vote);
        Task UpdateVoteCountsAsync(int reportId);

        Task AddAuditLogAsync(ReportAuditLog log);
        Task<IEnumerable<ReportAuditLog>> GetAuditLogsAsync(int reportId);

        Task<int> GetUserReportsCountTodayAsync(int userId);
    }
}