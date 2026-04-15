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

        // ────────────────────────────────────────────────────────────────────────
        // GET: api/v1/incident
        // Supports filtering, sorting, and pagination via query string.
        //
        // Filter params  : type, severity, status, checkpointId, createdAfter, createdBefore
        // Sort params    : sortBy (createdAt|updatedAt|severity|type|status)
        //                  sortOrder (asc|desc)
        // Pagination     : page (default 1), pageSize (default 10, max 50)
        //
        // Example:
        //   /incident?type=closure&severity=High&status=active
        //             &sortBy=createdAt&sortOrder=desc&page=1&pageSize=10
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<ActionResult<PagedResult<IncidentResponseDTO>>> GetAll(
            [FromQuery] IncidentQueryParams queryParams)
        {
            var pagedIncidents = await _incidentService.GetPagedAsync(queryParams);

            var result = new PagedResult<IncidentResponseDTO>
            {
                Data       = _mapper.Map<List<IncidentResponseDTO>>(pagedIncidents.Data),
                TotalCount = pagedIncidents.TotalCount,
                Page       = pagedIncidents.Page,
                PageSize   = pagedIncidents.PageSize
            };

            return Ok(result);
        }

        // GET: api/v1/incident/verified — Public (raw SQL, kept for compatibility)
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
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IncidentResponseDTO>> GetById(int id)
        {
            var incident = await _incidentService.GetByIdAsync(id);
            if (incident == null) return NotFound(new { message = $"Incident {id} not found." });
            var result = _mapper.Map<IncidentResponseDTO>(incident);
            return Ok(result);
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST: api/v1/incident — Admin or Moderator only
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin,moderator")]
        [HttpPost]
        public async Task<ActionResult<IncidentResponseDTO>> Create(IncidentCreateDTO incidentDto)
        {
            var incident = _mapper.Map<Incident>(incidentDto);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
                incident.CreatedByUserId = int.Parse(userIdClaim.Value);

            var created = await _incidentService.CreateAsync(incident);

            // Reload with navigation props for proper DTO mapping
            var loaded = await _incidentService.GetByIdAsync(created.IncidentId);
            var result = _mapper.Map<IncidentResponseDTO>(loaded);

            return CreatedAtAction(
                nameof(GetById),
                new { version = "1", id = result.IncidentId },
                result
            );
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT: api/v1/incident/{id} — Admin or Moderator only
        // Partial update: any combination of title, description, type,
        // severity, status, checkpointId can be provided.
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin,moderator")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<IncidentResponseDTO>> Update(int id, IncidentUpdateDTO dto)
        {
            var updated = await _incidentService.UpdatePartialAsync(id, dto);
            if (updated == null) return NotFound(new { message = $"Incident {id} not found." });
            var result = _mapper.Map<IncidentResponseDTO>(updated);
            return Ok(result);
        }

        // ────────────────────────────────────────────────────────────────────────
        // PATCH: api/v1/incident/{id}/verify — Admin or Moderator only
        // Sets status = "verified", stamps VerifiedByUserId from JWT.
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin,moderator")]
        [HttpPatch("{id:int}/verify")]
        public async Task<ActionResult<IncidentResponseDTO>> Verify(int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int verifiedByUserId = int.Parse(userIdClaim.Value);
            var incident = await _incidentService.VerifyAsync(id, verifiedByUserId);
            if (incident == null) return NotFound(new { message = $"Incident {id} not found." });

            var result = _mapper.Map<IncidentResponseDTO>(incident);
            return Ok(result);
        }

        // ────────────────────────────────────────────────────────────────────────
        // PATCH: api/v1/incident/{id}/close — Admin or Moderator only
        // Sets status = "closed".
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin,moderator")]
        [HttpPatch("{id:int}/close")]
        public async Task<ActionResult<IncidentResponseDTO>> Close(int id)
        {
            var incident = await _incidentService.CloseAsync(id);
            if (incident == null) return NotFound(new { message = $"Incident {id} not found." });

            var result = _mapper.Map<IncidentResponseDTO>(incident);
            return Ok(result);
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE: api/v1/incident/{id} — Admin only
        // ────────────────────────────────────────────────────────────────────────
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _incidentService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = $"Incident {id} not found." });
            return NoContent();
        }
    }
}