namespace TemplateProject.Core.Data;

public interface IUnitOfWork
{
    bool ShouldSaveChanges { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}