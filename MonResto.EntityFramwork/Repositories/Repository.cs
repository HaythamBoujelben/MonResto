
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

        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return  _context.Set<T>().FirstOrDefault(predicate);
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }

}
