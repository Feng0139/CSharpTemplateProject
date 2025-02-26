using System.Diagnostics;
using Autofac;
using TemplateProject.Core.Data;

namespace TemplateProject.IntegrationTest;

public class IntegrationTestBase
{
    private ILifetimeScope? _lifetimeScope;

    protected IntegrationTestBase(ILifetimeScope? lifetimeScope = null)
    {
        _lifetimeScope = lifetimeScope;
    }
    
    protected void SetupScope(ILifetimeScope lifetimeScope) => _lifetimeScope = lifetimeScope;

    protected async Task Run<T>(Func<T, Task> action, Action<ContainerBuilder>? extraRegistration = null)
        where T : notnull
    {
        if (_lifetimeScope == null) return;
        
        var dependency = extraRegistration != null
             ? _lifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
             : _lifetimeScope.BeginLifetimeScope().Resolve<T>();
     
         await action(dependency);
    }
    
    protected async Task Run<T, U>(Func<T, U, Task> action, Action<ContainerBuilder>? extraRegistration = null)
        where T : notnull
        where U : notnull
    {
        if (_lifetimeScope == null) return;

        var lifetime = extraRegistration != null
            ? _lifetimeScope.BeginLifetimeScope(extraRegistration)
            : _lifetimeScope.BeginLifetimeScope();
        
        var dependency = lifetime.Resolve<T>();
        var dependency2 = lifetime.Resolve<U>();
        
        await action(dependency, dependency2);
    }
    
    protected async Task RunWithUnitOfWork<T>(Func<T, Task> action, Action<ContainerBuilder>? extraRegistration = null)
        where T : notnull
    {
        if (_lifetimeScope == null) return;
        
        var scope = extraRegistration != null
            ? _lifetimeScope.BeginLifetimeScope(extraRegistration)
            : _lifetimeScope.BeginLifetimeScope();

        var dependency = scope.Resolve<T>();
        var unitOfWork = scope.Resolve<IUnitOfWork>();
        
        await action(dependency);
        await unitOfWork.SaveChangesAsync();
    }
}