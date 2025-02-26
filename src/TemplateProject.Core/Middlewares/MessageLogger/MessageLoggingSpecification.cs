using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using TemplateProject.Core.Data;
using TemplateProject.Core.Services.MessageLogging;
using TemplateProject.Message.Attributes;
using TemplateProject.Message.Commands.MessageLogging;

namespace TemplateProject.Core.Middlewares.MessageLogger;

public class MessageLoggingSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    private IMessageLoggingService _messageLoggingService;
    private IUnitOfWork _unitOfWork;

    public MessageLoggingSpecification(IMessageLoggingService messageLoggingService, IUnitOfWork unitOfWork)
    {
        _messageLoggingService = messageLoggingService;
        _unitOfWork = unitOfWork;
    }
    
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        if (context.Result == null) return false;
        
        return context.Message.GetType().GetCustomAttribute<MessageLoggingAttribute>() != null;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        if (!ShouldExecute(context, cancellationToken)) return;
        
        await _messageLoggingService.AddLogsAsync(new List<AddMessageLoggingCommand>
        {
            new()
            {
                MessageType = context.Message.GetType().ToString(),
                ResultType = context.Result.GetType().ToString(),
                MessageJson = JsonSerializer.Serialize(context.Message),
                ResultJson = JsonSerializer.Serialize(context.Result)
            }
        }, cancellationToken).ConfigureAwait(false);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        throw ex;
    }
}