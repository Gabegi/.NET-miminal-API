using System.Linq.Expressions;

namespace Infrastructure.Repositories;

/// <summary>
/// Generic repository interface providing CRUD operations and transaction management.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets all entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Gets an entity by its primary key.
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Finds entities matching the specified predicate.
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Adds multiple entities.
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Removes an entity by its primary key.
    /// </summary>
    Task<bool> RemoveAsync(int id);

    /// <summary>
    /// Removes a specific entity.
    /// </summary>
    Task RemoveAsync(T entity);

    /// <summary>
    /// Removes multiple entities.
    /// </summary>
    Task RemoveRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Checks if an entity exists matching the predicate.
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Gets the count of entities matching the predicate.
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// Saves all changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackTransactionAsync();
}
