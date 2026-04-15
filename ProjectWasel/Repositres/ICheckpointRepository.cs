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
        Task<List<Checkpoint>> GetActiveCheckpointsRawAsync();
        Task<Checkpoint?> GetByIdRawAsync(int id);
        Task<List<CheckpointStatusHistory>> GetStatusHistoryAsync(int checkpointId);
        Task<PagedResult<CheckpointStatusHistory>> GetStatusHistoryPagedAsync(int checkpointId, int page, int pageSize);
        Task<PagedResult<Checkpoint>> GetPagedAsync(CheckpointQueryParams queryParams);
        Task UpdateWithHistoryAsync(Checkpoint checkpoint);
        Task<Checkpoint?> UpdatePartialAsync(int id, CheckpointUpdateDTO dto);
    }

    public class CheckpointRepository : GenericRepository<Checkpoint>, ICheckpointRepository
    {
        private readonly WaselContext _context;

        public CheckpointRepository(WaselContext context) : base(context)
        {
            _context = context;
        }

        // ===== LINQ active filter =====
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

        // ===== Create with initial history entry =====
        public new async Task<Checkpoint> AddAsync(Checkpoint checkpoint)
        {
            _context.Checkpoints.Add(checkpoint);
            await _context.SaveChangesAsync();

            var initialHistory = new CheckpointStatusHistory
            {
                CheckpointId = checkpoint.CheckpointId,
                Status       = checkpoint.Status,
                ChangedAt    = DateTime.UtcNow
            };

            _context.CheckpointStatusHistories.Add(initialHistory);
            await _context.SaveChangesAsync();

            return checkpoint;
        }

        // ===== Status history — full list (legacy) =====
        public async Task<List<CheckpointStatusHistory>> GetStatusHistoryAsync(int checkpointId)
        {
            return await _context.CheckpointStatusHistories
                .Where(h => h.CheckpointId == checkpointId)
                .Include(h => h.Checkpoint)
                    .ThenInclude(c => c.Incidents)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        // ===== Status history — paginated =====
        public async Task<PagedResult<CheckpointStatusHistory>> GetStatusHistoryPagedAsync(
            int checkpointId, int page, int pageSize)
        {
            // Clamp page / pageSize
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50;

            var query = _context.CheckpointStatusHistories
                .Where(h => h.CheckpointId == checkpointId)
                .Include(h => h.Checkpoint)
                    .ThenInclude(c => c.Incidents)
                .OrderByDescending(h => h.ChangedAt);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<CheckpointStatusHistory>
            {
                Data       = data,
                TotalCount = totalCount,
                Page       = page,
                PageSize   = pageSize
            };
        }

        // ===== Full Filtering + Sorting + Pagination =====
        public async Task<PagedResult<Checkpoint>> GetPagedAsync(CheckpointQueryParams q)
        {
            var query = _dbSet.AsQueryable();

            // ── Filters ──────────────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(q.Status))
                query = query.Where(c => c.Status.ToLower() == q.Status.ToLower());

            if (!string.IsNullOrWhiteSpace(q.Name))
                query = query.Where(c => c.Name.ToLower().Contains(q.Name.ToLower()));

            // ── Sorting ───────────────────────────────────────────────────────────
            bool asc = string.Equals(q.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);

            query = q.SortBy?.ToLower() switch
            {
                "status"      => asc ? query.OrderBy(c => c.Status)      : query.OrderByDescending(c => c.Status),
                "lastupdated" => asc ? query.OrderBy(c => c.LastUpdated) : query.OrderByDescending(c => c.LastUpdated),
                _             => asc ? query.OrderBy(c => c.Name)        : query.OrderByDescending(c => c.Name),
            };

            // ── Pagination ────────────────────────────────────────────────────────
            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .ToListAsync();

            return new PagedResult<Checkpoint>
            {
                Data       = data,
                TotalCount = totalCount,
                Page       = q.Page,
                PageSize   = q.PageSize
            };
        }

        // ===== Update with history tracking (full object) =====
        public async Task UpdateWithHistoryAsync(Checkpoint updated)
        {
            var existing = await _context.Checkpoints.FindAsync(updated.CheckpointId);
            if (existing == null) return;

            if (existing.Status != updated.Status)
            {
                var history = new CheckpointStatusHistory
                {
                    CheckpointId = updated.CheckpointId,
                    Status       = updated.Status,
                    ChangedAt    = DateTime.UtcNow
                };
                _context.CheckpointStatusHistories.Add(history);
            }

            existing.Name        = updated.Name;
            existing.Latitude    = updated.Latitude;
            existing.Longitude   = updated.Longitude;
            existing.Status      = updated.Status;
            existing.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // ===== Partial update (status only) with history tracking =====
        public async Task<Checkpoint?> UpdatePartialAsync(int id, CheckpointUpdateDTO dto)
        {
            var existing = await _context.Checkpoints.FindAsync(id);
            if (existing == null) return null;

            if (existing.Status != dto.Status)
            {
                var history = new CheckpointStatusHistory
                {
                    CheckpointId = id,
                    Status       = dto.Status,
                    ChangedAt    = DateTime.UtcNow
                };
                _context.CheckpointStatusHistories.Add(history);
            }

            existing.Status      = dto.Status;
            existing.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return existing;
        }
    }
}