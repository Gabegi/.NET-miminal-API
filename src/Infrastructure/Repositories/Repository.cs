using System.Linq.Expressions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation providing CRUD operations and transaction management using Entity Framework Core.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;
    private IDbContextTransaction? _transaction;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
            return false;

        _dbSet.Remove(entity);
        return true;
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().AnyAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate is null
            ? await _dbSet.AsNoTracking().CountAsync()
            : await _dbSet.AsNoTracking().CountAsync(predicate);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
