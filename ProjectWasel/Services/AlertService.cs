using Microsoft.EntityFrameworkCore;
using ProjectWasel.Data;
using ProjectWasel.Models;
using ProjectWasel.Repositories;
using ProjectWasel.Repositres;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWasel.Services
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