using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using TemplateProject.Core.Data;
using TemplateProject.Core.Services.MessageLogging;

namespace TemplateProject.Core.Middlewares.MessageLogger;

public static class MessageLoggingMiddleware
{
    public static void UseMessageLogging<TContext>(
        this IPipeConfigurator<TContext> configurator, IMessageLoggingService? messageLoggingService = null, IUnitOfWork? unitOfWork = null)
        where TContext : IContext<IMessage>
    {
        if (configurator.DependencyScope == null)
        {
            throw new DependencyScopeNotConfiguredException(
                $"{nameof(messageLoggingService)} is not provided and IDependencyScope is not configured, Please ensure {nameof(messageLoggingService)} is registered properly if you are using IoC container, otherwise please pass {nameof(messageLoggingService)} as parameter");
        }
        
        messageLoggingService ??= configurator.DependencyScope.Resolve<IMessageLoggingService>();

        unitOfWork ??= configurator.DependencyScope.Resolve<IUnitOfWork>();
        
        configurator.AddPipeSpecification(new MessageLoggingSpecification<TContext>(messageLoggingService, unitOfWork));
    }
}