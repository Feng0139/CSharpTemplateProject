using System.Linq.Expressions;
using TemplateProject.Core.Domain;

namespace TemplateProject.Core.Data;

public interface IRepository
{
    Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;

    Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;
    
    Task<List<TEntity>> GetAllAsync<TEntity>(CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;

    Task<List<TEntity>> ToListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) where TEntity : class, IEntity;
    
    Task InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;

    Task InsertAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;

    Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;

    Task UpdateAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;

    Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) where TEntity : class, IEntity;
    
    Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;
    
    Task DeleteAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;
    
    Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;
    
    Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity;

    Task<List<T>> SqlQueryAsync<T>(string sql, params object[] parameters)
        where T : class, IEntity;

    IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class, IEntity;

    IQueryable<TEntity> QueryNoTracking<TEntity>(Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class, IEntity;
}