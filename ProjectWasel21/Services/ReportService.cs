using ProjectWasel21.Models;
using ProjectWasel21.Models.ModelsDTO;
using ProjectWasel21.Models.Repositres;

namespace ProjectWasel21.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repo;
        private const int MaxReportsPerDay = 10;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ReportResponseDTO>> GetReportsAsync(
            int page, int pageSize, string? category,
            string? status, string? sortBy)
        {
            var reports = await _repo.GetAllAsync(page, pageSize, category, status, sortBy);
            return reports.Select(MapToDTO);
        }

        public async Task<ReportResponseDTO?> GetReportByIdAsync(int id)
        {
            var report = await _repo.GetByIdAsync(id);
            return report == null ? null : MapToDTO(report);
        }

        public async Task<(bool Success, string Message, ReportResponseDTO? Data)>
            CreateReportAsync(CreateReportDTO dto, int userId)
        {
            var count = await _repo.GetUserReportsCountTodayAsync(userId);

            if (count >= MaxReportsPerDay)
                return (false, "Daily limit reached", null);

            var duplicate = await _repo.FindDuplicateAsync(
                dto.Latitude, dto.Longitude, dto.Category);

            if (duplicate != null)
                return (false, "Duplicate report found", MapToDTO(duplicate));

            var report = new Report
            {
                Latitude = (decimal)dto.Latitude,
                Longitude = (decimal)dto.Longitude,
                Category = dto.Category,
                Description = dto.Description,
                UserId = userId,
                IncidentId = dto.IncidentId, // 👈 مهم جداً
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(report);
            return (true, "Report created", MapToDTO(created));
        }

        public async Task<(bool Success, string Message)>
            VoteAsync(int reportId, int userId, bool isUpVote)
        {
            if (await _repo.HasUserVotedAsync(reportId, userId))
                return (false, "Already voted");

            await _repo.AddVoteAsync(new ReportVote
            {
                ReportId = reportId,
                UserId = userId,
                IsUpVote = isUpVote
            });

            await _repo.UpdateVoteCountsAsync(reportId);

            return (true, "Vote recorded");
        }

        public async Task<(bool Success, string Message)>
            ModerateReportAsync(int reportId, int moderatorId, ModerationDTO dto)
        {
            var report = await _repo.GetByIdAsync(reportId);
            if (report == null)
                return (false, "Not found");

            var prev = report.Status.ToString();

            report.Status = dto.Action switch
            {
                "Approve" => ReportStatus.Verified,
                "Reject" => ReportStatus.Rejected,
                "MarkDuplicate" => ReportStatus.Duplicate,
                _ => report.Status
            };

            await _repo.UpdateAsync(report);

            await _repo.AddAuditLogAsync(new ReportAuditLog
            {
                ReportId = reportId,
                ModeratorId = moderatorId,
                Action = dto.Action,
                Reason = dto.Reason,
                PreviousStatus = prev,
                NewStatus = report.Status.ToString()
            });

            return (true, "Updated successfully");
        }

        private static ReportResponseDTO MapToDTO(Report r) => new()
        {
            Id = r.ReportId,
            Latitude = (double)r.Latitude,
            Longitude = (double)r.Longitude,
            Category = r.Category,
            Description = r.Description,
            Status = r.Status.ToString(),

            // ✅ FIX HERE
            UpVotes = r.Votes?.Count(v => v.IsUpVote) ?? 0,

            User = r.User == null ? null : new UserDTO
            {
                UserId = r.User.UserId,
                Username = r.User.Username,
                Email = r.User.Email,
                Role = r.User.Role
            },

            Incident = r.Incident == null ? null : new IncidentNestedDTO
            {
                IncidentId = r.Incident.IncidentId,
                Title = r.Incident.Title,
                Type = r.Incident.Type,

                Checkpoint = r.Incident.Checkpoint == null ? null : new CheckpointDTO
                {
                    Name = r.Incident.Checkpoint.Name,
                    Latitude = r.Incident.Checkpoint.Latitude,
                    Longitude = r.Incident.Checkpoint.Longitude,
                    Status = r.Incident.Checkpoint.Status
                }
            }
        };
    }
}