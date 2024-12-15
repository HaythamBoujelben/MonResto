using Microsoft.EntityFrameworkCore.Storage;
using MonRestoAPI.Models;
using System;

namespace MonRestoAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly MonRestoAPIContext _context;
        private IDbContextTransaction _transaction;

        public IRepository<Article> Articles { get; private set; }
        public IRepository<UserProfile> UserProfiles { get; private set; }
        public IRepository<Cart> Carts { get; private set; }
        public IRepository<CartItem> CartItems { get; private set; }
        public IRepository<Order> Orders { get; private set; }
        public IRepository<OrderItem> OrderItems { get; private set; }
        public IRepository<Menu> Menus { get; private set; }
        public IRepository<Category> Categorys { get; private set; }

        public UnitOfWork(MonRestoAPIContext context)
        {
            _context = context;
            Articles = new Repository<Article>(_context);
            UserProfiles = new Repository<UserProfile>(_context);
            Carts = new Repository<Cart>(_context);
            CartItems = new Repository<CartItem>(_context);
            Orders = new Repository<Order>(_context);
            OrderItems = new Repository<OrderItem>(_context);
            Menus = new Repository<Menu>(_context);
            Categorys = new Repository<Category>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch
            {
                await RollbackAsync();
                throw; 
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }

}
