﻿using Microsoft.EntityFrameworkCore.Storage;
using MonRestoAPI.Models;
using System;

namespace MonRestoAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly MonRestoAPIContext _context;
        private IDbContextTransaction _transaction;

        public IRepository<Article> Articles { get; private set; }

        public UnitOfWork(MonRestoAPIContext context)
        {
            _context = context;
            Articles = new Repository<Article>(_context);
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
                throw; // Re-throw the exception to handle it in the calling code
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
