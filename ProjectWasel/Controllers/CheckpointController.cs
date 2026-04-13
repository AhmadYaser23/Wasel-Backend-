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

        // GET: api/v1/checkpoint — Public: anyone can list checkpoints
        [HttpGet]
        public async Task<ActionResult<List<Checkpoint>>> GetAll()
        {
            var checkpoints = await _checkpointRepo.GetAllAsync();
            return Ok(checkpoints);
        }

        // GET: api/v1/checkpoint/active — Public
        [HttpGet("active")]
        public async Task<ActionResult<List<Checkpoint>>> GetActive()
        {
            var activeCheckpoints = await _checkpointRepo.GetActiveCheckpointsAsync();
            return Ok(activeCheckpoints);
        }

        // GET: api/v1/checkpoint/{id} — Public
        [HttpGet("{id}")]
        public async Task<ActionResult<Checkpoint>> GetById(int id)
        {
            var checkpoint = await _checkpointRepo.GetByIdAsync(id);
            if (checkpoint == null) return NotFound();
            return Ok(checkpoint);
        }

        // GET: api/v1/checkpoint/active-raw — Public (Raw SQL)
        [HttpGet("active-raw")]
        public async Task<ActionResult<List<Checkpoint>>> GetActiveRaw()
        {
            var active = await _checkpointRepo.GetActiveCheckpointsRawAsync();
            return Ok(active);
        }

        // GET: api/v1/checkpoint/raw/{id} — Public (Raw SQL)
        [HttpGet("raw/{id}")]
        public async Task<ActionResult<Checkpoint>> GetByIdRaw(int id)
        {
            var checkpoint = await _checkpointRepo.GetByIdRawAsync(id);
            if (checkpoint == null) return NotFound();
            return Ok(checkpoint);
        }

        // GET: api/v1/checkpoint/{id}/history — Public: view status history
        [HttpGet("{id}/history")]
        public async Task<ActionResult<List<CheckpointStatusHistoryDTO>>> GetStatusHistory(int id)
        {
            var checkpoint = await _checkpointRepo.GetByIdAsync(id);
            if (checkpoint == null) return NotFound();

            var history = await _checkpointRepo.GetStatusHistoryAsync(id);

            var result = history.Select(h => new CheckpointStatusHistoryDTO
            {
                HistoryId = h.HistoryId,
                Status = h.Status,
                ChangedAt = h.ChangedAt,
                LastUpdated = h.Checkpoint?.LastUpdated ?? DateTime.MinValue,
                CheckpointName = h.Checkpoint?.Name ?? string.Empty,
                Latitude = h.Checkpoint?.Latitude ?? 0,
                Longitude = h.Checkpoint?.Longitude ?? 0,
                Incidents = h.Checkpoint?.Incidents?.Select(i => new IncidentSummaryDTO
                {
                    IncidentId = i.IncidentId,
                    Title = i.Title,
                    Type = i.Type,
                    Severity = i.Severity,
                    Status = i.Status
                }).ToList() ?? new List<IncidentSummaryDTO>()
            }).ToList();

            return Ok(result);
        }

        // POST: api/v1/checkpoint — Admin only
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<CheckpointDTO>> Create(CheckpointDTO checkpointDto)
        {
            var checkpoint = _mapper.Map<Checkpoint>(checkpointDto);
            await _checkpointRepo.AddAsync(checkpoint);

            var resultDto = _mapper.Map<CheckpointDTO>(checkpoint);
            return CreatedAtAction(nameof(GetById), new { id = checkpoint.CheckpointId }, resultDto);
        }

        // PUT: api/v1/checkpoint/{id} — Public: any user can update (auto-tracks status changes)
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CheckpointUpdateDTO dto)
        {
            var updated = await _checkpointRepo.UpdatePartialAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/v1/checkpoint/{id} — Admin only
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _checkpointRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}