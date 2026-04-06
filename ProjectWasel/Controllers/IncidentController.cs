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
        private readonly IMapper _mapper; // Added Mapper

        public IncidentController(IIncidentRepository incidentRepo, IMapper mapper) // Added mapper to constructor
        {
            _incidentRepo = incidentRepo;
            _mapper = mapper;
        }

        // GET: api/v1/incident
        [HttpGet]
        public async Task<ActionResult<List<Incident>>> GetAll()
        {
            var incidents = await _incidentRepo.GetAllAsync();
            return Ok(incidents);
        }

        // GET: api/v1/incident/verified
        [HttpGet("verified")]
        public async Task<ActionResult<List<Incident>>> GetVerified()
        {
            var verified = await _incidentRepo.GetVerifiedIncidentsRawAsync();
            return Ok(verified);
        }

        // GET: api/v1/incident/verified-raw
        [HttpGet("verified-raw")]
        public async Task<ActionResult<List<Incident>>> GetVerifiedRaw()
        {
            var verified = await _incidentRepo.GetVerifiedIncidentsRawAsync();
            return Ok(verified);
        }

        // GET: api/v1/incident/checkpoint/{checkpointId}
        [HttpGet("checkpoint/{checkpointId}")]
        public async Task<ActionResult<List<Incident>>> GetByCheckpoint(int checkpointId)
        {
            var list = await _incidentRepo.GetByCheckpointRawAsync(checkpointId);
            return Ok(list);
        }

        // GET: api/v1/incident/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetById(int id)
        {
            var incident = await _incidentRepo.GetByIdAsync(id);

            if (incident == null)
                return NotFound();

            return Ok(incident);
        }
        
        [HttpGet("filter")]
        public async Task<ActionResult> GetByType([FromQuery] string? type = null)
        {
            IEnumerable<Incident> incidents = await _incidentRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(type))
                incidents = incidents.Where(i => i.Type != null && i.Type.ToLower() == type.ToLower());

            return Ok(incidents);
        }
        

        // Updated POST to use the DTO for validation and mapping
        [HttpPost]
        public async Task<ActionResult<Incident>> Create(IncidentCreateDTO incidentDto)
        {
            // 1. Map DTO to Model
            var incident = _mapper.Map<Incident>(incidentDto);

            // 2. Set default timestamps
            incident.CreatedAt = DateTime.UtcNow;
            incident.UpdatedAt = DateTime.UtcNow;
            incident.Status = "active";

            // 3. Save
            await _incidentRepo.AddAsync(incident);

            return CreatedAtAction(
                nameof(GetById),
                new { version = "1", id = incident.IncidentId },
                incident
            );
        }

        // PUT: api/v1/incident/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Incident incident)
        {
            if (id != incident.IncidentId)
                return BadRequest();

            await _incidentRepo.UpdateAsync(incident);

            return NoContent();
        }

        // DELETE: api/v1/incident/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _incidentRepo.DeleteAsync(id);

            return NoContent();
        }
    }
}