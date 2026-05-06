using System.Linq.Expressions;

namespace BMD.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Get All
        Task<IEnumerable<T>> GetAllAsync();

        // Get By Id
        Task<T?> GetByIdAsync(int id);

        // Find with condition
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Add
        Task AddAsync(T entity);

        // Add Range
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update
        void Update(T entity);

        // Delete
        void Delete(T entity);

        // Delete By Id
        Task DeleteByIdAsync(int id);

        // Save Changes
        Task<int> SaveChangesAsync();
    }
}