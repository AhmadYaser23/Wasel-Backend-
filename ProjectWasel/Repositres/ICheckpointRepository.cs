using Microsoft.EntityFrameworkCore;
using ProjectWasel.Data;
using ProjectWasel.Models;
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
                .FromSqlRaw("SELECT * FROM Checkpoints WHERE Status = {0}", "active")
                .ToListAsync();
        }

        public async Task<Checkpoint?> GetByIdRawAsync(int id)
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM Checkpoints WHERE CheckpointId = {0}", id)
                .FirstOrDefaultAsync();
        }
        
        public  new async Task<Checkpoint> AddAsync(Checkpoint checkpoint)
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

    }
    
   
    
}