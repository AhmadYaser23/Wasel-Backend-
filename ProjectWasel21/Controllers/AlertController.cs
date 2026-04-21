using Microsoft.AspNetCore.Mvc;
using ProjectWasel21.Services;
using ProjectWasel21.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWasel21.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/alerts")]
    public class AlertController : ControllerBase
    {
        private readonly AlertService _alertService;

        public AlertController(AlertService alertService)
        {
            _alertService = alertService;
        }

        // ================= GET SUBSCRIBERS =================
        [HttpGet("subscribers/{incidentId}")]
        public async Task<ActionResult<object>> GetSubscribersForIncident(int incidentId)
        {
            var incident = await _alertService.GetIncidentByIdAsync(incidentId);

            if (incident == null)
            {
                return NotFound(new
                {
                    message = $"Incident with ID {incidentId} not found"
                });
            }

            var subscribers = await _alertService.GetSubscribersForIncident(incident);

            return Ok(new
            {
                data = subscribers,
                totalCount = subscribers.Count()
            });
        }
    }
}