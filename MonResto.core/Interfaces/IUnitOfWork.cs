using MonRestoAPI.Models;

namespace MonRestoAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Article> Articles { get; }
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<Cart> Carts { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Order> Orders { get;}
        IRepository<OrderItem> OrderItems { get; }
        IRepository<Menu> Menus { get; }
        IRepository<Category> Categorys { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

}
