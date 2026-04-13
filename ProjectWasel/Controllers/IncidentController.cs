using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Repositories;
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
        private readonly IIncidentRepository _incidentRepo;
        private readonly IMapper _mapper;

        public IncidentController(IIncidentRepository incidentRepo, IMapper mapper)
        {
            _incidentRepo = incidentRepo;
            _mapper = mapper;
        }

        // GET: api/v1/incident — Public
        [HttpGet]
        public async Task<ActionResult<List<Incident>>> GetAll()
        {
            var incidents = await _incidentRepo.GetAllAsync();
            return Ok(incidents);
        }

        // GET: api/v1/incident/verified — Public
        [HttpGet("verified")]
        public async Task<ActionResult<List<Incident>>> GetVerified()
        {
            var verified = await _incidentRepo.GetVerifiedIncidentsRawAsync();
            return Ok(verified);
        }

        // GET: api/v1/incident/verified-raw — Public (Raw SQL)
        [HttpGet("verified-raw")]
        public async Task<ActionResult<List<Incident>>> GetVerifiedRaw()
        {
            var verified = await _incidentRepo.GetVerifiedIncidentsRawAsync();
            return Ok(verified);
        }

        // GET: api/v1/incident/checkpoint/{checkpointId} — Public
        [HttpGet("checkpoint/{checkpointId}")]
        public async Task<ActionResult<List<Incident>>> GetByCheckpoint(int checkpointId)
        {
            var list = await _incidentRepo.GetByCheckpointRawAsync(checkpointId);
            return Ok(list);
        }

        // GET: api/v1/incident/{id} — Public
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetById(int id)
        {
            var incident = await _incidentRepo.GetByIdAsync(id);
            if (incident == null) return NotFound();
            return Ok(incident);
        }
        
        // GET: api/v1/incident/filter?type=closure&severity=High — Public
        [HttpGet("filter")]
        public async Task<ActionResult> GetFiltered([FromQuery] string? type = null, [FromQuery] string? severity = null)
        {
            var incidents = await _incidentRepo.GetFilteredAsync(type, severity);
            return Ok(incidents);
        }

        // POST: api/v1/incident — Admin or Moderator only
        [Authorize(Roles = "admin,moderator")]
        [HttpPost]
        public async Task<ActionResult<Incident>> Create(IncidentCreateDTO incidentDto)
        {
            var incident = _mapper.Map<Incident>(incidentDto);

            // Extract logged-in user's ID from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
                incident.CreatedByUserId = int.Parse(userIdClaim.Value);

            incident.CreatedAt = DateTime.UtcNow;
            incident.UpdatedAt = DateTime.UtcNow;
            incident.Status = "active";

            await _incidentRepo.AddAsync(incident);

            return CreatedAtAction(
                nameof(GetById),
                new { version = "1", id = incident.IncidentId },
                incident
            );
        }

        // PUT: api/v1/incident/{id} — Admin or Moderator only
        [Authorize(Roles = "admin,moderator")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, IncidentUpdateDTO dto)
        {
            var updated = await _incidentRepo.UpdatePartialAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/v1/incident/{id} — Admin only
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _incidentRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}