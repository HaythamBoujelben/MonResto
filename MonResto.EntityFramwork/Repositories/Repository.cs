using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MonRestoAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MonRestoAPIContext _context;

        public Repository(MonRestoAPIContext context)
        {
            _context = context;
        }

        // Add an entity to the database asynchronously
        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        // Delete an entity from the database
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        // Get all entities from the database
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        // Get an entity by its ID asynchronously
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // Find an entity using a predicate (synchronous)
        public T Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        // Find an entity using a predicate (asynchronous)
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        // Update an entity in the database
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
