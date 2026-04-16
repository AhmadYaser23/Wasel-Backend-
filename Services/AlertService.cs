using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Data;
using ProjectWasel21.Models;
using ProjectWasel21.Repositories;
using ProjectWasel21.Repositres;
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

        public async Task<Incident> GetIncidentByIdAsync(int id)
        {
            return await _context.Incidents.FindAsync(id);
        }

        public async Task<IEnumerable<Subscription>> GetSubscribersForIncident(Incident incident)
        {
            // مثال: تجيب كل المشتركين اللي منطقتهم أو فئتهم متوافقة مع الحادث
            return await _context.Subscriptions
                .Where(s => s.GeographicArea.Contains("Qalandia")) // ممكن تحسبي حسب incident
                .ToListAsync();
        }
    }
}