using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Data;
using ProjectWasel21.Models;
using ProjectWasel21.Models.Repositres;

namespace ProjectWasel21.Models.Repositres
{
    public class ReportRepository : IReportRepository
    {
        private readonly WaselContext _context;

        public ReportRepository(WaselContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        public async Task<IEnumerable<Report>> GetAllAsync(
            int page,
            int pageSize,
            string? category = null,
            string? status = null,
            string? sortBy = null)
        {
            var query = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Incident)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(r => r.Category == category);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(r => r.Status.ToString() == status);

            query = sortBy switch
            {
                "oldest" => query.OrderBy(r => r.CreatedAt),
                _ => query.OrderByDescending(r => r.CreatedAt)
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // ================= GET BY ID =================
        public async Task<Report?> GetByIdAsync(int id)
        {
            return await _context.Reports
                .Include(r => r.User)
                .Include(r => r.Incident)
                .FirstOrDefaultAsync(r => r.ReportId == id);
        }

        // ================= CREATE =================
        public async Task<Report> CreateAsync(Report report)
        {
            report.CreatedAt = DateTime.UtcNow;

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return report;
        }

        // ================= UPDATE =================
        public async Task<Report?> UpdateAsync(Report report)
        {
            var existing = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == report.ReportId);

            if (existing == null) return null;

            existing.Category = report.Category;
            existing.Description = report.Description;
            existing.Latitude = report.Latitude;
            existing.Longitude = report.Longitude;
            existing.IncidentId = report.IncidentId;
            existing.Status = report.Status;

            await _context.SaveChangesAsync();
            return existing;
        }

        // ================= DELETE =================
        public async Task<bool> DeleteAsync(int id)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == id);

            if (report == null) return false;

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        // ================= DUPLICATE CHECK =================
        public async Task<Report?> FindDuplicateAsync(
            double lat,
            double lng,
            string category,
            int radiusMeters = 500)
        {
            decimal radius = (decimal)(radiusMeters / 111000.0);
            var cutoff = DateTime.UtcNow.AddHours(-6);

            return await _context.Reports
                .FirstOrDefaultAsync(r =>
                    r.Category == category &&
                    r.CreatedAt >= cutoff &&
                    Math.Abs((double)(r.Latitude - (decimal)lat)) <= (double)radius &&
                    Math.Abs((double)(r.Longitude - (decimal)lng)) <= (double)radius
                );
        }

        // ================= VOTING =================
        public async Task<bool> HasUserVotedAsync(int reportId, int userId)
        {
            return await _context.ReportVotes
                .AnyAsync(v => v.ReportId == reportId && v.UserId == userId);
        }

        public async Task AddVoteAsync(ReportVote vote)
        {
            _context.ReportVotes.Add(vote);
            await _context.SaveChangesAsync();
        }

        // ================= UPDATE VOTE COUNT (FIXED) =================
        public async Task UpdateVoteCountsAsync(int reportId)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == reportId);

            if (report == null) return;

            // نحسب عدد الـ upvotes
            var upVotesCount = await _context.ReportVotes
                .CountAsync(v => v.ReportId == reportId && v.IsUpVote);

            // ❗ ما بنحطه داخل Votes لأنه ICollection (خطأ عندك سابقاً)
            // إذا بدك تعرضه استخدمه بالـ DTO أو API response

            Console.WriteLine($"Report {reportId} UpVotes = {upVotesCount}");
        }

        // ================= AUDIT LOG =================
        public async Task AddAuditLogAsync(ReportAuditLog log)
        {
            _context.ReportAuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReportAuditLog>> GetAuditLogsAsync(int reportId)
        {
            return await _context.ReportAuditLogs
                .Include(a => a.Moderator)
                .Where(a => a.ReportId == reportId)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();
        }

        // ================= TODAY COUNT =================
        public async Task<int> GetUserReportsCountTodayAsync(int userId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Reports
                .CountAsync(r => r.UserId == userId && r.CreatedAt >= today);
        }
    }
}