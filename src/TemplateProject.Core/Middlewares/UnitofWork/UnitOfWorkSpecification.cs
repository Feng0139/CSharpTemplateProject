using System.Runtime.ExceptionServices;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using TemplateProject.Core.Data;

namespace TemplateProject.Core.Middlewares.UnitofWork;

public class UnitOfWorkSpecification<TContext>(IUnitOfWork unitOfWork) : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return context.Message is ICommand or IEvent;
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
        if (ShouldExecute(context, cancellationToken))
        {
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            unitOfWork.ShouldSaveChanges = false;
        }
    }

    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        throw ex;
    }
}