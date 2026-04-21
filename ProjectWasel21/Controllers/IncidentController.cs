using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWasel21.Models;
using ProjectWasel21.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectWasel21.Models.ModelsDTO;

namespace ProjectWasel21.Controllers
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

        // ================= GET ALL =================
        [Authorize] // 🔥 أي مستخدم مسجل
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] IncidentQueryParams queryParams)
        {
            var paged = await _incidentService.GetPagedAsync(queryParams);

            return Ok(new
            {
                success = true,
                data = _mapper.Map<List<IncidentResponseDTO>>(paged.Data),
                pagination = new
                {
                    paged.Page,
                    paged.PageSize,
                    paged.TotalCount
                }
            });
        }

        // ================= GET VERIFIED =================
        [Authorize]
        [HttpGet("verified")]
        public async Task<ActionResult> GetVerified()
        {
            var verified = await _incidentService.GetVerifiedIncidentsRawAsync();

            return Ok(new
            {
                success = true,
                data = _mapper.Map<List<IncidentResponseDTO>>(verified)
            });
        }

        // ================= GET BY CHECKPOINT =================
        [Authorize]
        [HttpGet("checkpoint/{checkpointId}")]
        public async Task<ActionResult> GetByCheckpoint(int checkpointId)
        {
            var list = await _incidentService.GetByCheckpointRawAsync(checkpointId);

            return Ok(new
            {
                success = true,
                data = _mapper.Map<List<IncidentResponseDTO>>(list)
            });
        }

        // ================= GET BY ID =================
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var incident = await _incidentService.GetByIdAsync(id);

            if (incident == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Incident {id} not found"
                });

            return Ok(new
            {
                success = true,
                data = _mapper.Map<IncidentResponseDTO>(incident)
            });
        }

        // ================= CREATE =================
        [Authorize(Roles = "admin,moderator")]
        [HttpPost]
        public async Task<ActionResult> Create(IncidentCreateDTO dto)
        {
            var incident = _mapper.Map<Incident>(dto);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { success = false, message = "Unauthorized" });

            incident.CreatedByUserId = int.Parse(userIdClaim.Value);

            var created = await _incidentService.CreateAsync(incident);
            var loaded = await _incidentService.GetByIdAsync(created.IncidentId);

            return Ok(new
            {
                success = true,
                message = "Incident created successfully",
                data = _mapper.Map<IncidentResponseDTO>(loaded)
            });
        }

        // ================= UPDATE =================
        [Authorize(Roles = "admin,moderator")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, IncidentUpdateDTO dto)
        {
            var updated = await _incidentService.UpdatePartialAsync(id, dto);

            if (updated == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Incident {id} not found"
                });

            return Ok(new
            {
                success = true,
                message = "Incident updated successfully",
                data = _mapper.Map<IncidentResponseDTO>(updated)
            });
        }

        // ================= VERIFY =================
        [Authorize(Roles = "admin,moderator")]
        [HttpPatch("{id:int}/verify")]
        public async Task<ActionResult> Verify(int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var incident = await _incidentService.VerifyAsync(id, userId);

            if (incident == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Incident {id} not found"
                });

            return Ok(new
            {
                success = true,
                message = "Incident verified successfully",
                data = _mapper.Map<IncidentResponseDTO>(incident)
            });
        }

        // ================= CLOSE =================
        [Authorize(Roles = "admin,moderator")]
        [HttpPatch("{id:int}/close")]
        public async Task<ActionResult> Close(int id)
        {
            var incident = await _incidentService.CloseAsync(id);

            if (incident == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Incident {id} not found"
                });

            return Ok(new
            {
                success = true,
                message = "Incident closed successfully",
                data = _mapper.Map<IncidentResponseDTO>(incident)
            });
        }

        // ================= DELETE =================
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _incidentService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new
                {
                    success = false,
                    message = $"Incident {id} not found"
                });

            return Ok(new
            {
                success = true,
                message = "Incident deleted successfully",
                incidentId = id
            });
        }
    }
}