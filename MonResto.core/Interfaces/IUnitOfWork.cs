using MonRestoAPI.Models;

namespace MonRestoAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Article> Articles { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

}
