using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Data;
using ProjectWasel21.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWasel21.Services
{
    public class AlertService
    {
        private readonly WaselContext _context;

        public AlertService(WaselContext context)
        {
            _context = context;
        }

        // ================= GET INCIDENT =================
        public async Task<Incident?> GetIncidentByIdAsync(int id)
        {
            return await _context.Incidents
                .Include(i => i.Checkpoint)
                .FirstOrDefaultAsync(i => i.IncidentId == id);
        }

        // ================= GET SUBSCRIBERS =================
        public async Task<IEnumerable<Subscription>> GetSubscribersForIncident(Incident incident)
        {
            if (incident == null)
                return new List<Subscription>();

            var checkpointName = incident.Checkpoint?.Name;
            var category = incident.Type;

            if (string.IsNullOrEmpty(checkpointName) || string.IsNullOrEmpty(category))
                return new List<Subscription>();

            var subscribers = await _context.Subscriptions
                .Where(s =>
                    s.GeographicArea == checkpointName ||
                    s.Category == category
                )
                .ToListAsync();

            return subscribers;
        }

        // ================= CREATE ALERT LOGIC =================
        public async Task<List<Subscription>> GenerateAlertsForIncident(int incidentId)
        {
            var incident = await GetIncidentByIdAsync(incidentId);

            if (incident == null)
                return new List<Subscription>();

            var subscribers = await GetSubscribersForIncident(incident);

            // هون لاحقاً ممكن تضيف:
            // - إرسال Notifications
            // - Email / Push notifications
            // - Save Alert Logs

            return subscribers.ToList();
        }
    }
}