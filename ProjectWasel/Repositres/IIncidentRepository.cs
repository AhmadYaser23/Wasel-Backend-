using Microsoft.EntityFrameworkCore;
using ProjectWasel.Data;
using ProjectWasel.Models;
using ProjectWasel.Repositres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel.Repositories
{
    public interface IIncidentRepository : IRepository<Incident>
    {
        Task<List<Incident>> GetVerifiedIncidentsRawAsync();
        Task<List<Incident>> GetByCheckpointRawAsync(int checkpointId);
    }

    public class IncidentRepository : GenericRepository<Incident>, IIncidentRepository
    {
        private readonly WaselContext _context;

        public IncidentRepository(WaselContext context) : base(context) 
        {
            _context = context;
        }

        // ===== Raw SQL Methods =====
        public async Task<List<Incident>> GetVerifiedIncidentsRawAsync()
        {
            return await _context.Incidents
                .FromSqlRaw("SELECT * FROM Incidents WHERE Status = {0}", "verified")
                .ToListAsync();
        }

        public async Task<List<Incident>> GetByCheckpointRawAsync(int checkpointId)
        {
            return await _context.Incidents
                .FromSqlRaw("SELECT * FROM Incidents WHERE CheckpointId = {0}", checkpointId)
                .ToListAsync();
        }
        
        public async Task<List<Incident>> GetIncidentsByCheckpointAsync(int checkpointId)
        {
            return await _dbSet
                .Where(i => i.CheckpointId == checkpointId && i.Status == "active")
                .OrderByDescending(i => i.CreatedAt) // Newest first!
                .ToListAsync();
        }
       
    }
}