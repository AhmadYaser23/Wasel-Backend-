using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectWasel.Models.ModelsDTO;
using AutoMapper;
using static ProjectWasel.Repositories.ICheckpointRepository;

namespace ProjectWasel.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/checkpoint")]
    public class CheckpointController : ControllerBase
    {
        private readonly ICheckpointRepository _checkpointRepo;
        private readonly IMapper _mapper;

        public CheckpointController(ICheckpointRepository checkpointRepo, IMapper mapper)
        {
            _checkpointRepo = checkpointRepo;
            _mapper = mapper;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET: api/v1/checkpoint
        // Supports filtering, sorting, and pagination via query string.
        //
        // Filter params : status (active|closed|delayed), name (partial search)
        // Sort params   : sortBy (name|status|lastUpdated), sortOrder (asc|desc)
        // Pagination    : page (default 1), pageSize (default 10, max 50)
        //
        // Example:
        //   /checkpoint?status=active&name=Qalandia
        //               &sortBy=lastUpdated&sortOrder=desc&page=1&pageSize=10
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<ActionResult<PagedResult<Checkpoint>>> GetAll(
            [FromQuery] CheckpointQueryParams queryParams)
        {
            var result = await _checkpointRepo.GetPagedAsync(queryParams);
            return Ok(result);
        }

        // GET: api/v1/checkpoint/active — Public (LINQ, no pagination needed)
        [HttpGet("active")]
        public async Task<ActionResult<List<Checkpoint>>> GetActive()
        {
            var activeCheckpoints = await _checkpointRepo.GetActiveCheckpointsAsync();
            return Ok(activeCheckpoints);
        }

        // GET: api/v1/checkpoint/active-raw — Public (Raw SQL)
        [HttpGet("active-raw")]
        public async Task<ActionResult<List<Checkpoint>>> GetActiveRaw()
        {
            var active = await _checkpointRepo.GetActiveCheckpointsRawAsync();
            return Ok(active);
        }

        // GET: api/v1/checkpoint/raw/{id} — Public (Raw SQL)
        [HttpGet("raw/{id:int}")]
        public async Task<ActionResult<Checkpoint>> GetByIdRaw(int id)
        {
            var checkpoint = await _checkpointRepo.GetByIdRawAsync(id);
            if (checkpoint == null) return NotFound(new { message = $"Checkpoint {id} not found." });
            return Ok(checkpoint);
        }

        // GET: api/v1/checkpoint/{id} — Public
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Checkpoint>> GetById(int id)
        {
            var checkpoint = await _checkpointRepo.GetByIdAsync(id);
            if (checkpoint == null) return NotFound(new { message = $"Checkpoint {id} not found." });
            return Ok(checkpoint);
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET: api/v1/checkpoint/{id}/history
        // Returns paginated status history for a checkpoint, newest first.
        //
        // Pagination: page (default 1), pageSize (default 10, max 50)
        //
        // Example: /checkpoint/1/history?page=1&pageSize=5
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("{id:int}/history")]
        public async Task<ActionResult<PagedResult<CheckpointStatusHistoryDTO>>> GetStatusHistory(
            int id,
            [FromQuery] int page     = 1,
            [FromQuery] int pageSize = 10)
        {
            var checkpoint = await _checkpointRepo.GetByIdAsync(id);
            if (checkpoint == null) return NotFound(new { message = $"Checkpoint {id} not found." });

            var pagedHistory = await _checkpointRepo.GetStatusHistoryPagedAsync(id, page, pageSize);

            // Map to DTO
            var mappedData = pagedHistory.Data.Select(h => new CheckpointStatusHistoryDTO
            {
                HistoryId       = h.HistoryId,
                Status          = h.Status,
                ChangedAt       = h.ChangedAt,
                LastUpdated     = h.Checkpoint?.LastUpdated ?? DateTime.MinValue,
                CheckpointName  = h.Checkpoint?.Name ?? string.Empty,
                Latitude        = h.Checkpoint?.Latitude ?? 0,
                Longitude       = h.Checkpoint?.Longitude ?? 0,
                Incidents       = h.Checkpoint?.Incidents?.Select(i => new IncidentSummaryDTO
                {
                    IncidentId = i.IncidentId,
                    Title      = i.Title,
                    Type       = i.Type,
                    Severity   = i.Severity,
                    Status     = i.Status
                }).ToList() ?? new List<IncidentSummaryDTO>()
            }).ToList();

            var result = new PagedResult<CheckpointStatusHistoryDTO>
            {
                Data       = mappedData,
                TotalCount = pagedHistory.TotalCount,
                Page       = pagedHistory.Page,
                PageSize   = pagedHistory.PageSize
            };

            return Ok(result);
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST: api/v1/checkpoint — Admin only
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<CheckpointDTO>> Create(CheckpointDTO checkpointDto)
        {
            var checkpoint = _mapper.Map<Checkpoint>(checkpointDto);
            await _checkpointRepo.AddAsync(checkpoint);

            var resultDto = _mapper.Map<CheckpointDTO>(checkpoint);
            return CreatedAtAction(nameof(GetById), new { id = checkpoint.CheckpointId }, resultDto);
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT: api/v1/checkpoint/{id} — Admin or Moderator only
        // Updates checkpoint status and auto-logs the change in history.
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin,moderator")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, CheckpointUpdateDTO dto)
        {
            var updated = await _checkpointRepo.UpdatePartialAsync(id, dto);
            if (updated == null) return NotFound(new { message = $"Checkpoint {id} not found." });
            return Ok(updated);
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE: api/v1/checkpoint/{id} — Admin only
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _checkpointRepo.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = $"Checkpoint {id} not found." });
            return NoContent();
        }
    }
}