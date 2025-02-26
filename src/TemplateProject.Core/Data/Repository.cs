using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TemplateProject.Core.Domain;

namespace TemplateProject.Core.Data;

public class Repository(TemplateProjectDbContext dbContext) : IRepository
{
    public async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return await dbContext.Set<TEntity>().SingleOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<TEntity>> GetAllAsync<TEntity>(CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return await dbContext.Set<TEntity>().ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<TEntity>> ToListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return await dbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        await dbContext.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        dbContext.ShouldSaveChanges = true;
    }

    public async Task InsertAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        await dbContext.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        dbContext.ShouldSaveChanges = true;
    }

    public Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        dbContext.Update(entity);
        dbContext.ShouldSaveChanges = true;
        return Task.CompletedTask;
    }

    public Task UpdateAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        dbContext.UpdateRange(entities);
        dbContext.ShouldSaveChanges = true;
        return Task.CompletedTask;
    }

    public async Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        await dbContext.Set<TEntity>().Where(predicate).ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
        dbContext.ShouldSaveChanges = true;
    }
    
    public Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        dbContext.Remove(entity);
        dbContext.ShouldSaveChanges = true;
        return Task.CompletedTask;
    }

    public Task DeleteAllAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        dbContext.RemoveRange(entities);
        dbContext.ShouldSaveChanges = true;
        return Task.CompletedTask;
    }
    
    public async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return await dbContext.Set<TEntity>().CountAsync(predicate, cancellationToken).ConfigureAwait(false);
    }
    
    public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return await dbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<List<TEntity>> SqlQueryAsync<TEntity>(string sql, params object[] parameters)
        where TEntity : class, IEntity
    {
        return await dbContext.Set<TEntity>().FromSqlRaw(sql, parameters).ToListAsync().ConfigureAwait(false);
    }

    public IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class, IEntity
    {
        return predicate == null ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().Where(predicate);
    }

    public IQueryable<TEntity> QueryNoTracking<TEntity>(Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class, IEntity
    {
        return predicate == null
            ? dbContext.Set<TEntity>().AsNoTracking()
            : dbContext.Set<TEntity>().AsNoTracking().Where(predicate);
    }
}