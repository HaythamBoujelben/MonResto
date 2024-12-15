using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MonRestoAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MonRestoAPIContext _context;
        private IQueryable<T> _query;

        public Repository(MonRestoAPIContext context)
        {
            _context = context;
            _query = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task<int> AddAndGetIdAsync(T entity)
        {
            await _context.AddAsync(entity); 
            await _context.SaveChangesAsync(); 
            var idProperty = typeof(T).GetProperty("Id"); 
            if (idProperty == null)
            {
                throw new InvalidOperationException($"The entity type {typeof(T).Name} does not have an 'Id' property.");
            }
            return (int)idProperty.GetValue(entity); 
        }


        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            var result = _query.ToList();
            _query = _context.Set<T>();
            return _context.Set<T>().ToList();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public IRepository<T> Include(Expression<Func<T, object>> navigationProperty)
        {
            _query = _query.Include(navigationProperty);
            return this;
        }
    }
}
