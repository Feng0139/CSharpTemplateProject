using System.Net;
using System.Net.Http.Json;
using TemplateProject.Message.Dto;

namespace TemplateProject.Core.Extension;

public static class HttpClientExtension
{
    public static async Task<TemplateProjectResponse<T>> GetAsync<T>(this HttpClient client, string url, CancellationToken cancellationToken)
    {
        return await SafeProcessAsync(async () =>
        {
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            
            var data = await ReadResponseContentAsAsync<T>(response, cancellationToken).ConfigureAwait(false);
            
            return new TemplateProjectResponse<T>
            {
                Code = response.StatusCode,
                Data = data
            };
        }, cancellationToken).ConfigureAwait(false);
    }
    
    public static async Task<TemplateProjectResponse<T>> PostAsync<T>(this HttpClient client, string url, HttpContent content, CancellationToken cancellationToken)
    {
        return await SafeProcessAsync(async () =>
        {
            var response = await client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
            
            var data = await ReadResponseContentAsAsync<T>(response, cancellationToken).ConfigureAwait(false);
            
            return new TemplateProjectResponse<T>
            {
                Code = response.StatusCode,
                Data = data
            };
        }, cancellationToken).ConfigureAwait(false);
    }
    
    
    public static async Task<TemplateProjectResponse<T>> PostAsync<T>(this HttpClient client, string url, object content, CancellationToken cancellationToken)
    {
        return await SafeProcessAsync(async () =>
        {
            var response = await client.PostAsJsonAsync(url, content, cancellationToken).ConfigureAwait(false);
            
            var data = await ReadResponseContentAsAsync<T>(response, cancellationToken).ConfigureAwait(false);

            return new TemplateProjectResponse<T>
            {
                Code = response.StatusCode,
                Data = data
            };
        }, cancellationToken).ConfigureAwait(false);
    }
    
    public static async Task<TemplateProjectResponse<T>> SafeProcessAsync<T>(Func<Task<TemplateProjectResponse<T>>> func, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            return await func().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return new TemplateProjectResponse<T>
            {
                Code = HttpStatusCode.BadRequest,
                Message = ex.Message
            };
        }
    }
    
    public static async Task<T> ReadResponseContentAsAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (typeof(T) == typeof(string))
            return (T)(object) await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        
        if (typeof(T) == typeof(byte))
            return (T)(object) await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        
        if (typeof(T) == typeof(Stream))
            return (T)(object) await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException();
    }
}