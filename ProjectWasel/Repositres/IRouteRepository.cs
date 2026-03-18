using Microsoft.EntityFrameworkCore;
using ProjectWasel.Models;
using ProjectWasel.Data;
using ProjectWasel.Repositres;
using System.Collections.Generic;
using System.Threading.Tasks;
using Route = ProjectWasel.Models.Route;

namespace ProjectWasel.Repositories
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