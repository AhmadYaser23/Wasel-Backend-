using Microsoft.EntityFrameworkCore;
using ProjectWasel.Data;
using ProjectWasel.Models;
using ProjectWasel.Models.ModelsDTO;
using ProjectWasel.Repositres;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWasel.Repositories
{
    public interface ICheckpointRepository : IRepository<Checkpoint>
    {
        Task<List<Checkpoint>> GetActiveCheckpointsAsync();
        Task<List<Checkpoint>> GetActiveCheckpointsRawAsync(); // Raw SQL
        Task<Checkpoint?> GetByIdRawAsync(int id);             // Raw SQL By Id
        Task<List<CheckpointStatusHistory>> GetStatusHistoryAsync(int checkpointId);
        Task UpdateWithHistoryAsync(Checkpoint checkpoint);
        Task<Checkpoint?> UpdatePartialAsync(int id, CheckpointUpdateDTO dto);
    }

    public class CheckpointRepository : GenericRepository<Checkpoint>, ICheckpointRepository
    {
        public CheckpointRepository(WaselContext context) : base(context) { }

        // LINQ
        public async Task<List<Checkpoint>> GetActiveCheckpointsAsync()
        {
            return await _dbSet.Where(c => c.Status == "active").ToListAsync();
        }

        // ===== Raw SQL =====
        public async Task<List<Checkpoint>> GetActiveCheckpointsRawAsync()
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM \"Checkpoints\" WHERE \"Status\" = {0}", "active")
                .ToListAsync();
        }

        public async Task<Checkpoint?> GetByIdRawAsync(int id)
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM \"Checkpoints\" WHERE \"CheckpointId\" = {0}", id)
                .FirstOrDefaultAsync();
        }
        
        public new async Task<Checkpoint> AddAsync(Checkpoint checkpoint)
        {
            _context.Checkpoints.Add(checkpoint);
            await _context.SaveChangesAsync();

            var initialHistory = new CheckpointStatusHistory
            {
                CheckpointId = checkpoint.CheckpointId, 
                Status = checkpoint.Status,             
                ChangedAt = DateTime.UtcNow             
            };

            _context.CheckpointStatusHistories.Add(initialHistory);
            await _context.SaveChangesAsync();

            return checkpoint;
        }

        // Get full status history for a checkpoint (ordered by date)
        public async Task<List<CheckpointStatusHistory>> GetStatusHistoryAsync(int checkpointId)
        {
            return await _context.CheckpointStatusHistories
                .Where(h => h.CheckpointId == checkpointId)
                .Include(h => h.Checkpoint)
                    .ThenInclude(c => c.Incidents)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        // Update checkpoint and auto-track status change in history
        public async Task UpdateWithHistoryAsync(Checkpoint updated)
        {
            var existing = await _context.Checkpoints.FindAsync(updated.CheckpointId);
            if (existing == null) return;

            // If status changed, record it in history
            if (existing.Status != updated.Status)
            {
                var history = new CheckpointStatusHistory
                {
                    CheckpointId = updated.CheckpointId,
                    Status = updated.Status,
                    ChangedAt = DateTime.UtcNow
                };
                _context.CheckpointStatusHistories.Add(history);
            }

            // Update fields
            existing.Name = updated.Name;
            existing.Latitude = updated.Latitude;
            existing.Longitude = updated.Longitude;
            existing.Status = updated.Status;
            existing.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<Checkpoint?> UpdatePartialAsync(int id, CheckpointUpdateDTO dto)
        {
            var existing = await _context.Checkpoints.FindAsync(id);
            if (existing == null) return null;

            // Track status change in history if it actually changed
            if (existing.Status != dto.Status)
            {
                var history = new CheckpointStatusHistory
                {
                    CheckpointId = id,
                    Status = dto.Status,
                    ChangedAt = DateTime.UtcNow
                };
                _context.CheckpointStatusHistories.Add(history);
            }

            existing.Status = dto.Status;
            existing.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return existing;
        }
    }
}