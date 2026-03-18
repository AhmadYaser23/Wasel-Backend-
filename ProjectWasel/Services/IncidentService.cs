using ProjectWasel.Models;
using ProjectWasel.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel.Services
{
    public class IncidentService
    {
        private readonly IIncidentRepository _incidentRepo;

        public IncidentService(IIncidentRepository incidentRepo)
        {
            _incidentRepo = incidentRepo;
        }

        public Task<Incident> GetByIdAsync(int id) => _incidentRepo.GetByIdAsync(id);

        public async Task<Incident> AddIncidentAsync(Incident incident)
        {
            var added = await _incidentRepo.AddAsync(incident);

            // هنا ممكن نضيف Trigger للـAlerts إذا الحادث verified
            // await _alertService.TriggerAlertForIncident(added);

            return added;
        }

        public Task UpdateIncidentAsync(Incident incident) => _incidentRepo.UpdateAsync(incident);

        public Task DeleteIncidentAsync(int id) => _incidentRepo.DeleteAsync(id);

        public Task<List<Incident>> GetAllAsync() => _incidentRepo.GetAllAsync();

        // ===== استدعاء Raw SQL =====
        public Task<List<Incident>> GetVerifiedIncidentsRawAsync() => _incidentRepo.GetVerifiedIncidentsRawAsync();
        public Task<List<Incident>> GetByCheckpointRawAsync(int checkpointId) => _incidentRepo.GetByCheckpointRawAsync(checkpointId);
    }
}