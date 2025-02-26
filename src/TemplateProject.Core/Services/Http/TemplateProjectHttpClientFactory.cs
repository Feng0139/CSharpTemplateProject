using System.Net.Http.Headers;
using Autofac;
using TemplateProject.Core.Extension;
using TemplateProject.Message.Dto;
using TemplateProject.Message.Dto.HttpClient;

namespace TemplateProject.Core.Services.Http;

public interface ITemplateProjectHttpClientFactory : IScope
{
    HttpClient CreateClient(TokenModel? tokenModel = null, Dictionary<string, string>? headers = null, TimeSpan? timeout = null, bool beginScope = false);
    
    Task<TemplateProjectResponse<T>> GetAsync<T>(string url, CancellationToken cancellationToken, TimeSpan? timeout = null, TokenModel? tokenModel = null, Dictionary<string, string>? headers = null, bool beginScope = false);
    
    Task<TemplateProjectResponse<T>> PostAsync<T>(string url, HttpContent content, CancellationToken cancellationToken, TimeSpan? timeout = null, TokenModel? tokenModel = null, Dictionary<string, string>? headers = null, bool beginScope = false);
    
    Task<TemplateProjectResponse<T>> PostAsync<T>(string url, object content, CancellationToken cancellationToken, TimeSpan? timeout = null, TokenModel? tokenModel = null, Dictionary<string, string>? headers = null, bool beginScope = false);
}

public class TemplateProjectHttpClientFactory(ILifetimeScope scope) : ITemplateProjectHttpClientFactory
{
    public HttpClient CreateClient(TokenModel? tokenModel = null, Dictionary<string, string>? headers = null, TimeSpan? timeout = null, bool beginScope = false)
    {
        var scope1 = beginScope ? scope.BeginLifetimeScope() : scope;

        var client = scope1.TryResolve(out IHttpClientFactory? httpClientFactory)
            ? httpClientFactory.CreateClient()
            : new HttpClient();
        
        if (timeout != null)
            client.Timeout = timeout.Value;

        if (tokenModel != null)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenModel.Scheme, tokenModel.Parameter);

        if (headers == null) return client;
        
        foreach (var header in headers)
        {
            client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        return client;
    }
    
    public async Task<TemplateProjectResponse<T>> GetAsync<T>(string url, CancellationToken cancellationToken, TimeSpan? timeout = null, TokenModel? token = null, Dictionary<string, string>? headers = null, bool beginScope = false)
    {
        var client = CreateClient(token, headers, timeout, beginScope);
        
        var response = await client.GetAsync<T>(url, cancellationToken).ConfigureAwait(false);

        return response;
    }

    public async Task<TemplateProjectResponse<T>> PostAsync<T>(string url, HttpContent content, CancellationToken cancellationToken, TimeSpan? timeout = null, TokenModel? token = null, Dictionary<string, string>? headers = null, bool beginScope = false)
    {
        var client = CreateClient(token, headers, timeout, beginScope);
        
        var response = await client.PostAsync<T>(url, content, cancellationToken).ConfigureAwait(false);

        return response;
    }

    public async Task<TemplateProjectResponse<T>> PostAsync<T>(string url, object content, CancellationToken cancellationToken, TimeSpan? timeout = null, TokenModel? token = null, Dictionary<string, string>? headers = null, bool beginScope = false)
    {
        var client = CreateClient(token, headers, timeout, beginScope);
        
        var response = await client.PostAsync<T>(url, content, cancellationToken).ConfigureAwait(false);

        return response;
    }
}