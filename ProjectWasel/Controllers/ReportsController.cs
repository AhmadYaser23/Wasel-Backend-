using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models.ModelsDTO;
using ProjectWasel.Models.Repositres;
using ProjectWasel.Services;
using System.Security.Claims;

namespace ProjectWasel.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _service;
        private readonly IReportRepository _repo;

        public ReportsController(ReportService service, IReportRepository repo)
        {
            _service = service;
            _repo = repo;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? category = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sortBy = null)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Invalid pagination");

            var reports = await _service.GetReportsAsync(page, pageSize, category, status, sortBy);
            return Ok(new { page, pageSize, data = reports });
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var report = await _service.GetReportByIdAsync(id);

            if (report == null)
                return NotFound(new { message = "Report not found" });

            return Ok(report);
        }

        // ================= CREATE REPORT =================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReportDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetCurrentUserId();

            // 🔥 FIX: لو ما في user من التوكن استخدم واحد موجود
            if (userId == 0)
                userId = 3; // 👈 غيّرها حسب UserId الموجود عندك

            var (success, message, data) =
                await _service.CreateReportAsync(dto, userId);

            if (!success)
                return Conflict(new { message, duplicateReport = data });

            return CreatedAtAction(
                nameof(GetById),
                new { id = data!.Id },
                data
            );
        }

        // ================= VOTE =================
        [HttpPost("{id}/vote")]
        public async Task<IActionResult> Vote(int id, [FromBody] VoteDTO dto)
        {
            int userId = GetCurrentUserId();

            if (userId == 0)
                userId = 3;

            var (success, message) =
                await _service.VoteAsync(id, userId, dto.IsUpVote);

            return success
                ? Ok(new { message })
                : BadRequest(new { message });
        }

        // ================= MODERATE =================
        [HttpPost("{id}/moderate")]
        public async Task<IActionResult> Moderate(int id, [FromBody] ModerationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int moderatorId = GetCurrentUserId();

            if (moderatorId == 0)
                moderatorId = 3;

            var (success, message) =
                await _service.ModerateReportAsync(id, moderatorId, dto);

            return success
                ? Ok(new { message })
                : NotFound(new { message });
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.DeleteAsync(id);

            return result
                ? Ok(new { message = "Deleted successfully" })
                : NotFound(new { message = "Report not found" });
        }

        // ================= AUDIT LOGS =================
        [HttpGet("{id}/audit-logs")]
        public async Task<IActionResult> GetAuditLogs(int id)
        {
            var logs = await _repo.GetAuditLogsAsync(id);

            var result = logs.Select(l => new AuditLogResponseDTO
            {
                Id = l.Id,
                Action = l.Action,
                Reason = l.Reason,
                PreviousStatus = l.PreviousStatus,
                NewStatus = l.NewStatus,
                ActionDate = l.ActionDate,
                ModeratorName = l.Moderator?.Username ?? "System"
            });

            return Ok(result);
        }

        // ================= HELPER =================
        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
    }
}