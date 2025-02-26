using System.Net;
using Mediator.Net.Contracts;

namespace TemplateProject.Message.Dto;

public class TemplateProjectResponse<T> : TemplateProjectResponse
{
    public T? Data { get; set; }
}

public class TemplateProjectResponse : IResponse
{
    public HttpStatusCode Code { get; set; }

    public string Message { get; set; } = string.Empty;
}