using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWasel.Controllers
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
        public async Task<ActionResult> GetSubscribersForIncident([FromRoute] int incidentId)
        {
            // جلب الحادث من قاعدة البيانات
            var incident = await _alertService.GetIncidentByIdAsync(incidentId);
            if (incident == null)
                return NotFound($"Incident with ID {incidentId} not found.");

            // جلب المشتركين المتعلقين بالحادث
            var subscribers = await _alertService.GetSubscribersForIncident(incident);

            return Ok(new { data = subscribers, totalCount = subscribers.Count() });
        }

    }
}