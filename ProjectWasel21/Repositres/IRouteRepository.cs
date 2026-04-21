using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Models;
using ProjectWasel21.Data;
using ProjectWasel21.Repositres;
using System.Collections.Generic;
using System.Threading.Tasks;
using Route = ProjectWasel21.Models.Route;

namespace ProjectWasel21.Repositories
{
    public interface IRouteRepository : IRepository<Route>
    {
        Task<List<Route>> GetAllRawAsync();
        Task<Route?> GetByIdRawAsync(int id);
    }

    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        public RouteRepository(WaselContext context) : base(context) { }

        // ===== Raw SQL =====
        public async Task<List<Route>> GetAllRawAsync()
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM Routes")
                .ToListAsync();
        }

        public async Task<Route?> GetByIdRawAsync(int id)
        {
            return await _dbSet
                .FromSqlRaw("SELECT * FROM Routes WHERE RouteId = {0}", id)
                .FirstOrDefaultAsync();
        }
    }
}