using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Services;
using AutoMapper; 
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectWasel.Models.ModelsDTO;

namespace ProjectWasel.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/incident")]
    public class IncidentController : ControllerBase
    {
        private readonly IncidentService _incidentService;
        private readonly IMapper _mapper;

        public IncidentController(IncidentService incidentService, IMapper mapper)
        {
            _incidentService = incidentService;
            _mapper = mapper;
        }

        // GET: api/v1/incident — Public
        [HttpGet]
        public async Task<ActionResult<List<IncidentResponseDTO>>> GetAll()
        {
            var incidents = await _incidentService.GetAllAsync();
            var result = _mapper.Map<List<IncidentResponseDTO>>(incidents);
            return Ok(result);
        }

        // GET: api/v1/incident/verified — Public
        [HttpGet("verified")]
        public async Task<ActionResult<List<IncidentResponseDTO>>> GetVerified()
        {
            var verified = await _incidentService.GetVerifiedIncidentsRawAsync();
            var result = _mapper.Map<List<IncidentResponseDTO>>(verified);
            return Ok(result);
        }

        // GET: api/v1/incident/checkpoint/{checkpointId} — Public
        [HttpGet("checkpoint/{checkpointId}")]
        public async Task<ActionResult<List<IncidentResponseDTO>>> GetByCheckpoint(int checkpointId)
        {
            var list = await _incidentService.GetByCheckpointRawAsync(checkpointId);
            var result = _mapper.Map<List<IncidentResponseDTO>>(list);
            return Ok(result);
        }

        // GET: api/v1/incident/{id} — Public
        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentResponseDTO>> GetById(int id)
        {
            var incident = await _incidentService.GetByIdAsync(id);
            if (incident == null) return NotFound();
            var result = _mapper.Map<IncidentResponseDTO>(incident);
            return Ok(result);
        }
        
        // GET: api/v1/incident/filter?type=closure&severity=High — Public
        [HttpGet("filter")]
        public async Task<ActionResult<List<IncidentResponseDTO>>> GetFiltered([FromQuery] string? type = null, [FromQuery] string? severity = null)
        {
            var incidents = await _incidentService.GetFilteredAsync(type, severity);
            var result = _mapper.Map<List<IncidentResponseDTO>>(incidents);
            return Ok(result);
        }

        // POST: api/v1/incident — Admin or Moderator only
        [Authorize(Roles = "admin,moderator")]
        [HttpPost]
        public async Task<ActionResult<IncidentResponseDTO>> Create(IncidentCreateDTO incidentDto)
        {
            var incident = _mapper.Map<Incident>(incidentDto);

            // Extract logged-in user's ID from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
                incident.CreatedByUserId = int.Parse(userIdClaim.Value);

            var created = await _incidentService.CreateAsync(incident);

            // Reload with includes for proper DTO mapping
            var loaded = await _incidentService.GetByIdAsync(created.IncidentId);
            var result = _mapper.Map<IncidentResponseDTO>(loaded);

            return CreatedAtAction(
                nameof(GetById),
                new { version = "1", id = result.IncidentId },
                result
            );
        }

        // PUT: api/v1/incident/{id} — Admin or Moderator only
        [Authorize(Roles = "admin,moderator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<IncidentResponseDTO>> Update(int id, IncidentUpdateDTO dto)
        {
            var updated = await _incidentService.UpdatePartialAsync(id, dto);
            if (updated == null) return NotFound();
            var result = _mapper.Map<IncidentResponseDTO>(updated);
            return Ok(result);
        }

        // PATCH: api/v1/incident/{id}/verify — Admin or Moderator only
        [Authorize(Roles = "admin,moderator")]
        [HttpPatch("{id}/verify")]
        public async Task<ActionResult<IncidentResponseDTO>> Verify(int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int verifiedByUserId = int.Parse(userIdClaim.Value);
            var incident = await _incidentService.VerifyAsync(id, verifiedByUserId);
            if (incident == null) return NotFound();

            var result = _mapper.Map<IncidentResponseDTO>(incident);
            return Ok(result);
        }

        // PATCH: api/v1/incident/{id}/close — Admin or Moderator only
        [Authorize(Roles = "admin,moderator")]
        [HttpPatch("{id}/close")]
        public async Task<ActionResult<IncidentResponseDTO>> Close(int id)
        {
            var incident = await _incidentService.CloseAsync(id);
            if (incident == null) return NotFound();

            var result = _mapper.Map<IncidentResponseDTO>(incident);
            return Ok(result);
        }

        // DELETE: api/v1/incident/{id} — Admin only
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _incidentService.DeleteAsync(id);
            return NoContent();
        }
    }
}