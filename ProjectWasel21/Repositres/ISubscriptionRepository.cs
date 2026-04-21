using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Data;
using ProjectWasel21.Models;
using ProjectWasel21.Repositres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel21.Repositories
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task<List<Subscription>> GetByUserAsync(int userId);
        Task<List<Subscription>> GetByAreaAsync(string area);
        Task<List<Subscription>> GetByUserRawAsync(int userId);
        Task<List<Subscription>> GetByAreaRawAsync(string area);
        Task<Subscription?> GetByIdAsync(int id);
    }

    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(WaselContext context) : base(context) { }

        // GET ALL BY USER
        public async Task<List<Subscription>> GetByUserAsync(int userId)
        {
            return await _dbSet
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        // GET ALL BY AREA
        public async Task<List<Subscription>> GetByAreaAsync(string area)
        {
            return await _dbSet
                .Include(s => s.User)
                .Where(s => s.GeographicArea == area)
                .ToListAsync();
        }

        // RAW SQL BY USER
        public async Task<List<Subscription>> GetByUserRawAsync(int userId)
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM Subscriptions WHERE UserId = {0}", userId)
                .Include(s => s.User)
                .ToListAsync();
        }

        // RAW SQL BY AREA
        public async Task<List<Subscription>> GetByAreaRawAsync(string area)
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM Subscriptions WHERE GeographicArea = {0}", area)
                .Include(s => s.User)
                .ToListAsync();
        }

        // GET BY ID (FIXED - أهم وحدة عندك)
        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.SubscriptionId == id);
        }

        public async Task<List<Subscription>> GetAllAsync()
        {
            return await _dbSet
                .Include(s => s.User)
                .ToListAsync();
        }
    }
    }