using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;

namespace TemplateProject.Core.Caching;

public class MemoryCacheManager(IMemoryCache memoryCache) : ICacheManager
{
    public Task<T?> Get<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        memoryCache.TryGetValue<T>(key, out var result);

        return Task.FromResult(result);
    }

    public Task Set(string key, object data, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        if (expiry == null)
        {
            memoryCache.Set(key, data);
        }
        else
        {
            memoryCache.Set(key, data, expiry.Value);
        }

        return Task.CompletedTask;
    }

    public Task Remove(string key, CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(key);

        return Task.CompletedTask;
    }

    public Task RemoveByPrefix(string pattern, CancellationToken cancellationToken = default)
    {
        foreach (var key in GetKeys(memoryCache, pattern))
        {
            memoryCache.Remove(key);
        }

        return Task.CompletedTask;
    }

    public Task Clear(CancellationToken cancellationToken = default)
    {
        foreach (var key in GetKeys(memoryCache))
        {
            memoryCache.Remove(key);
        }
        
        return Task.CompletedTask;
    }

    public void Dispose() { }

    public static List<string> GetKeys(IMemoryCache memoryCache, string pattern = "")
    {
        var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
        var keys = new List<string>();
        
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var coherentState = memoryCache.GetType().GetField("_coherentState", flags)?.GetValue(memoryCache);
        var entries = coherentState?.GetType().GetField("_entries", flags)?.GetValue(coherentState);

        if (entries is IDictionary cacheItems)
        {
            keys.AddRange(cacheItems.Keys.Cast<string>().Where(x => string.IsNullOrEmpty(pattern) || regex.IsMatch(x)));
        }

        return keys;
    }
}