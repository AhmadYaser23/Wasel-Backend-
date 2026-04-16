using Microsoft.AspNetCore.Mvc;
using ProjectWasel21.Models;
using ProjectWasel21.Services;
using System.Collections.Generic;
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

        [HttpGet("subscribers/{incidentId}")]
        [ProducesResponseType(typeof(IEnumerable<Subscription>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscribersForIncident([FromRoute] int incidentId)
        {
            // جلب الحادث
            var incident = await _alertService.GetIncidentByIdAsync(incidentId);
            if (incident == null)
                return NotFound($"Incident with ID {incidentId} not found.");

            // جلب المشتركين
            var subscribers = await _alertService.GetSubscribersForIncident(incident);

            return Ok(subscribers); // ✅ يرجع List بشكل صحيح
        }
    }
}