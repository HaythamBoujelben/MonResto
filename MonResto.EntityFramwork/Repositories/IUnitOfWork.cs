using MonRestoAPI.Models;

namespace MonRestoAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

}
