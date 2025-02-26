using System.ComponentModel.DataAnnotations;

namespace TemplateProject.Core.Extension;

public static class PageExtension
{
    public static PagedList<TEntity> ToPaged<TEntity>(this IEnumerable<TEntity> source, int pageIndex, int pageSize) where TEntity : new()
    {
        var list = source.ToList();
        
        var total = list.Count;
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        
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

public class PageInput
{
    public int PageIndex { get; set; } = 1;
    
    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
    public int PageSize { get; set; } = 20;
}

public class PagedList<TEntity> where TEntity : new()
{
    public PagedList()
    {
        Items = new List<TEntity>();
    }
    
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }
    
    public int Total { get; set; }
    
    public int TotalPages { get; set; }
    
    public IEnumerable<TEntity> Items { get; set; }
    
    public bool HasPrevPage { get; set; }

    public bool HasNextPage { get; set; }
}