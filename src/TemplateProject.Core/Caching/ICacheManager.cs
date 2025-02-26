using TemplateProject.Core.Services;

namespace TemplateProject.Core.Caching;

public interface ICacheManager : IScope, IDisposable
{
    Task<T?> Get<T>(string key, CancellationToken cancellationToken = default) where T : class;
    
    Task Set(string key, object data, TimeSpan? expiry = null, CancellationToken cancellationToken = default);
    
    Task Remove(string key, CancellationToken cancellationToken = default);
    
    Task RemoveByPrefix(string pattern, CancellationToken cancellationToken = default);
    
    Task Clear(CancellationToken cancellationToken = default);
}