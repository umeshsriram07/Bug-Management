using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BMD.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BMDDbContext _context;

        private readonly DbSet<T> _dbSet;

        public Repository(BMDDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Get All
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Get By Id
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Find
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // Add
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Add Range
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // Update
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Delete
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Delete By Id
        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                Delete(entity);
            }
        }

        // Save Changes
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}