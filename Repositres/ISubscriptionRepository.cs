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
    }

    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(WaselContext context) : base(context) { }

        // باستخدام LINQ
        public async Task<List<Subscription>> GetByUserAsync(int userId)
        {
            return await _dbSet
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Subscription>> GetByAreaAsync(string area)
        {
            return await _dbSet
                .Where(s => s.GeographicArea == area)
                .ToListAsync();
        }

        // ===== باستخدام Raw SQL =====
        public async Task<List<Subscription>> GetByUserRawAsync(int userId)
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM Subscriptions WHERE UserId = {0}", userId)
                .ToListAsync();
        }

        public async Task<List<Subscription>> GetByAreaRawAsync(string area)
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM Subscriptions WHERE GeographicArea = {0}", area)
                .ToListAsync();
        }
    }
}