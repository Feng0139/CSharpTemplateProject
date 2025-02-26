using System.ComponentModel.DataAnnotations;

namespace TemplateProject.Message.Dto;

public class PageInputDto
{
    public int PageIndex { get; set; } = 1;
    
    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
    public int PageSize { get; set; } = 20;
}

public class PagedListDto<TEntity> where TEntity : new()
{
    public PagedListDto()
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