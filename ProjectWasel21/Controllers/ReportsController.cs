using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWasel21.Models.ModelsDTO;
using ProjectWasel21.Models.Repositres;
using ProjectWasel21.Services;
using System.Security.Claims;

namespace ProjectWasel21.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/reports")]
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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? category = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sortBy = null)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid pagination"
                });

            var reports = await _service.GetReportsAsync(page, pageSize, category, status, sortBy);

            return Ok(new
            {
                success = true,
                data = reports,
                pagination = new { page, pageSize }
            });
        }

        // ================= GET BY ID =================
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var report = await _service.GetReportByIdAsync(id);

            if (report == null)
                return NotFound(new
                {
                    success = false,
                    message = "Report not found"
                });

            return Ok(new
            {
                success = true,
                data = report
            });
        }

        // ================= CREATE =================
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReportDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid data",
                    errors = ModelState
                });

            int userId = GetCurrentUserId();

            if (userId == 0)
                return Unauthorized(new
                {
                    success = false,
                    message = "User not logged in"
                });

            var (success, message, data) =
                await _service.CreateReportAsync(dto, userId);

            if (!success)
                return Conflict(new
                {
                    success = false,
                    message,
                    duplicateReport = data
                });

            return Ok(new
            {
                success = true,
                message = "Report created successfully",
                data
            });
        }

        // ================= VOTE =================
        [Authorize]
        [HttpPost("{id}/vote")]
        public async Task<IActionResult> Vote(int id, [FromBody] VoteDTO dto)
        {
            int userId = GetCurrentUserId();

            if (userId == 0)
                return Unauthorized(new
                {
                    success = false,
                    message = "User not logged in"
                });

            var (success, message) =
                await _service.VoteAsync(id, userId, dto.IsUpVote);

            return success
                ? Ok(new
                {
                    success = true,
                    message
                })
                : BadRequest(new
                {
                    success = false,
                    message
                });
        }

        // ================= MODERATE =================
        [Authorize(Roles = "admin,moderator")]
        [HttpPost("{id}/moderate")]
        public async Task<IActionResult> Moderate(int id, [FromBody] ModerationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid data"
                });

            int moderatorId = GetCurrentUserId();

            if (moderatorId == 0)
                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid moderator"
                });

            var (success, message) =
                await _service.ModerateReportAsync(id, moderatorId, dto);

            return success
                ? Ok(new
                {
                    success = true,
                    message
                })
                : NotFound(new
                {
                    success = false,
                    message
                });
        }

        // ================= DELETE =================
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.DeleteAsync(id);

            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Report not found"
                });

            return Ok(new
            {
                success = true,
                message = "Report deleted successfully",
                reportId = id
            });
        }

        // ================= AUDIT LOGS =================
        [Authorize(Roles = "admin,moderator")]
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

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        // ================= HELPER =================
        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
    }
}