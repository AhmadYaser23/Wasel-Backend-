using ProjectWasel.Models;
using ProjectWasel.Models.ModelsDTO;
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

        public Task<List<Incident>> GetAllAsync() => _incidentRepo.GetAllAsync();

        public Task<Incident?> GetByIdAsync(int id) => _incidentRepo.GetByIdAsync(id);

        public Task<List<Incident>> GetVerifiedIncidentsRawAsync() => _incidentRepo.GetVerifiedIncidentsRawAsync();

        public Task<List<Incident>> GetByCheckpointRawAsync(int checkpointId) => _incidentRepo.GetByCheckpointRawAsync(checkpointId);

        public Task<List<Incident>> GetFilteredAsync(string? type, string? severity) => _incidentRepo.GetFilteredAsync(type, severity);

        public async Task<Incident> CreateAsync(Incident incident)
        {
            incident.CreatedAt = DateTime.UtcNow;
            incident.UpdatedAt = DateTime.UtcNow;
            incident.Status = "active";

            return await _incidentRepo.AddAsync(incident);
        }

        public Task<Incident?> UpdatePartialAsync(int id, IncidentUpdateDTO dto) => _incidentRepo.UpdatePartialAsync(id, dto);

        public Task<Incident?> VerifyAsync(int id, int verifiedByUserId) => _incidentRepo.VerifyAsync(id, verifiedByUserId);

        public Task<Incident?> CloseAsync(int id) => _incidentRepo.CloseAsync(id);

        public Task<bool> DeleteAsync(int id) => _incidentRepo.DeleteAsync(id);
    }
}