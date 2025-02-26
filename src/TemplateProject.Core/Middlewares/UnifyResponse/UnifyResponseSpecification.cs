using System.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using TemplateProject.Message.Dto;

namespace TemplateProject.Core.Middlewares.UnifyResponse;

public class UnifyResponseSpecification<TContext> : IPipeSpecification<TContext> where TContext : IContext<IMessage>
{
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return true;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        if (!ShouldExecute(context, cancellationToken) || context.Result is not TemplateProjectResponse) return Task.CompletedTask;
        
        var response = (dynamic)context.Result;
        
        if (response.Code == 0) response.Code = HttpStatusCode.OK;
        if (string.IsNullOrEmpty(response.Message)) response.Message = nameof(HttpStatusCode.OK).ToLower();

        return Task.CompletedTask;
    }

    public Task OnException(Exception ex, TContext context)
    {
        if (!ShouldExecute(context, default) ||  context.Result is not TemplateProjectResponse) throw ex;
        
        var response = (dynamic)context.Result;

        response.Code = HttpStatusCode.InternalServerError;
        response.Message = ex.Message;

        throw ex;
    }
}