using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Data;
using ProjectWasel21.Models;
using ProjectWasel21.Models.ModelsDTO;
using ProjectWasel21.Repositres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel21.Repositories
{
    public interface IIncidentRepository : IRepository<Incident>
    {
        Task<List<Incident>> GetVerifiedIncidentsRawAsync();
        Task<List<Incident>> GetByCheckpointRawAsync(int checkpointId);
        Task<List<Incident>> GetFilteredAsync(string? type, string? severity);
        Task<PagedResult<Incident>> GetPagedAsync(IncidentQueryParams queryParams);
        Task<Incident?> UpdatePartialAsync(int id, IncidentUpdateDTO dto);
        Task<Incident?> VerifyAsync(int id, int verifiedByUserId);
        Task<Incident?> CloseAsync(int id);
    }

    public class IncidentRepository : GenericRepository<Incident>, IIncidentRepository
    {
        private readonly WaselContext _context;

        public IncidentRepository(WaselContext context) : base(context)
        {
            _context = context;
        }

        // ===== Override base methods to include navigation =====
        public new async Task<List<Incident>> GetAllAsync()
        {
            return await _dbSet
                .Include(i => i.Checkpoint)
                .Include(i => i.CreatedByUser)
                .Include(i => i.VerifiedByUser)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public new async Task<Incident?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(i => i.Checkpoint)
                .Include(i => i.CreatedByUser)
                .Include(i => i.VerifiedByUser)
                .FirstOrDefaultAsync(i => i.IncidentId == id);
        }

        // ===== Raw SQL Methods =====
        public async Task<List<Incident>> GetVerifiedIncidentsRawAsync()
        {
            return await _context.Incidents
                .FromSqlRaw("SELECT * FROM \"Incidents\" WHERE \"Status\" = {0}", "verified")
                .ToListAsync();
        }

        public async Task<List<Incident>> GetByCheckpointRawAsync(int checkpointId)
        {
            return await _context.Incidents
                .FromSqlRaw("SELECT * FROM \"Incidents\" WHERE \"CheckpointId\" = {0}", checkpointId)
                .ToListAsync();
        }

        public async Task<List<Incident>> GetIncidentsByCheckpointAsync(int checkpointId)
        {
            return await _dbSet
                .Where(i => i.CheckpointId == checkpointId && i.Status == "active")
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Incident>> GetFilteredAsync(string? type, string? severity)
        {
            var query = _dbSet
                .Include(i => i.Checkpoint)
                .Include(i => i.CreatedByUser)
                .Include(i => i.VerifiedByUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(i => i.Type.ToLower() == type.ToLower());

            if (!string.IsNullOrEmpty(severity))
                query = query.Where(i => i.Severity.ToLower() == severity.ToLower());

            return await query.OrderByDescending(i => i.CreatedAt).ToListAsync();
        }

        // ===== Full Filtering + Sorting + Pagination =====
        public async Task<PagedResult<Incident>> GetPagedAsync(IncidentQueryParams q)
        {
            var query = _dbSet
                .Include(i => i.Checkpoint)
                .Include(i => i.CreatedByUser)
                .Include(i => i.VerifiedByUser)
                .AsQueryable();

            // ── Filters ──────────────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(q.Type))
                query = query.Where(i => i.Type.ToLower() == q.Type.ToLower());

            if (!string.IsNullOrWhiteSpace(q.Severity))
                query = query.Where(i => i.Severity.ToLower() == q.Severity.ToLower());

            if (!string.IsNullOrWhiteSpace(q.Status))
                query = query.Where(i => i.Status.ToLower() == q.Status.ToLower());

            if (q.CheckpointId.HasValue)
                query = query.Where(i => i.CheckpointId == q.CheckpointId.Value);

            if (q.CreatedAfter.HasValue)
                query = query.Where(i => i.CreatedAt >= q.CreatedAfter.Value.ToUniversalTime());

            if (q.CreatedBefore.HasValue)
                query = query.Where(i => i.CreatedAt <= q.CreatedBefore.Value.ToUniversalTime());

            // ── Sorting ───────────────────────────────────────────────────────────
            bool asc = string.Equals(q.SortOrder, "asc", StringComparison.OrdinalIgnoreCase);

            query = q.SortBy?.ToLower() switch
            {
                "updatedat" => asc ? query.OrderBy(i => i.UpdatedAt) : query.OrderByDescending(i => i.UpdatedAt),
                "severity" => asc ? query.OrderBy(i => i.Severity) : query.OrderByDescending(i => i.Severity),
                "type" => asc ? query.OrderBy(i => i.Type) : query.OrderByDescending(i => i.Type),
                "status" => asc ? query.OrderBy(i => i.Status) : query.OrderByDescending(i => i.Status),
                _ => asc ? query.OrderBy(i => i.CreatedAt) : query.OrderByDescending(i => i.CreatedAt),
            };

            // ── Pagination ────────────────────────────────────────────────────────
            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .ToListAsync();

            return new PagedResult<Incident>
            {
                Data = data,
                TotalCount = totalCount,
                Page = q.Page,
                PageSize = q.PageSize
            };
        }

        public async Task<Incident?> UpdatePartialAsync(int id, IncidentUpdateDTO dto)
        {
            var existing = await _context.Incidents
                .Include(i => i.Checkpoint)
                .Include(i => i.CreatedByUser)
                .Include(i => i.VerifiedByUser)
                .FirstOrDefaultAsync(i => i.IncidentId == id);
            if (existing == null) return null;

            if (dto.Title != null) existing.Title = dto.Title;
            if (dto.Description != null) existing.Description = dto.Description;
            if (dto.Type != null) existing.Type = dto.Type;
            if (dto.Severity != null) existing.Severity = dto.Severity;
            if (dto.Status != null) existing.Status = dto.Status;
            if (dto.CheckpointId.HasValue) existing.CheckpointId = dto.CheckpointId.Value;

            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<Incident?> VerifyAsync(int id, int verifiedByUserId)
        {
            var existing = await _context.Incidents
                .Include(i => i.Checkpoint)
                .Include(i => i.CreatedByUser)
                .Include(i => i.VerifiedByUser)
                .FirstOrDefaultAsync(i => i.IncidentId == id);
            if (existing == null) return null;

            existing.Status = "verified";
            existing.VerifiedByUserId = verifiedByUserId;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await _context.Entry(existing).Reference(i => i.VerifiedByUser).LoadAsync();

            return existing;
        }

        public async Task<Incident?> CloseAsync(int id)
        {
            var existing = await _context.Incidents
                .Include(i => i.Checkpoint)
                .Include(i => i.CreatedByUser)
                .Include(i => i.VerifiedByUser)
                .FirstOrDefaultAsync(i => i.IncidentId == id);
            if (existing == null) return null;

            existing.Status = "closed";
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return existing;
        }
    }
}