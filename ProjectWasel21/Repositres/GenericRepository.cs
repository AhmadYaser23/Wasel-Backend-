using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel21.Repositres
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly WaselContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(WaselContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();
        

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;



        }





    }



}