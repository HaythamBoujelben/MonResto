using System.Linq.Expressions;

namespace MonRestoAPI.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        T Find(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        Task<IEnumerable<T>> GetAllAsync();
    }

}
