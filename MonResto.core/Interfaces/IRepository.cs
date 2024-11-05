using System.Linq.Expressions;

namespace MonRestoAPI.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        T Find(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

}
