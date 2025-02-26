using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TemplateProject.Core.Extension;

public static class EfCorePagedExtension
{
    public static IQueryable<T> WhereIF<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
    
    public static PagedList<TEntity> ToPaged<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize) where TEntity : new()
    {
        var total = query.Count();
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        
        var hasPrevPage = pageIndex > 1;
        var hasNextPage = pageIndex < totalPages;

        return new PagedList<TEntity>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Total = total,
            TotalPages = totalPages,
            Items = items,
            HasPrevPage = hasPrevPage,
            HasNextPage = hasNextPage
        };
    }
    
    public static async Task<PagedList<TEntity>> ToPagedAsync<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize, CancellationToken cancellationToken) where TEntity : new()
    {
        var total = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

        var hasPrevPage = pageIndex > 1;
        var hasNextPage = pageIndex < totalPages;
        
        return new PagedList<TEntity>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Total = total,
            TotalPages = totalPages,
            Items = items,
            HasPrevPage = hasPrevPage,
            HasNextPage = hasNextPage
        };
    }
}