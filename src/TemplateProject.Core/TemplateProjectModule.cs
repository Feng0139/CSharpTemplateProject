using System.Reflection;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Mediator.Net;
using Mediator.Net.Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Serilog;
using TemplateProject.Core.Data;
using TemplateProject.Core.Jobs;
using TemplateProject.Core.Middlewares.Logger;
using TemplateProject.Core.Middlewares.MessageLogger;
using TemplateProject.Core.Middlewares.UnifyResponse;
using TemplateProject.Core.Middlewares.UnitofWork;
using TemplateProject.Core.Services;
using TemplateProject.Core.Settings;
using Module = Autofac.Module;

namespace TemplateProject.Core;

public class TemplateProjectModule(ILogger logger, IConfiguration configuration, params Assembly[] assemblies) : Module
{
    private readonly Assembly[] _assemblies = assemblies.Length == 0
        ? new Assembly[] { typeof(TemplateProjectModule).Assembly }
        : assemblies.Concat(new[] { typeof(TemplateProjectModule).Assembly }).Distinct().ToArray();
    
    protected override void Load(ContainerBuilder builder)
    {
        RegisterLogger(builder);
        
        RegisterMediator(builder);

        RegisterAutoMapper(builder);

        RegisterDependency(builder);
        
        RegisterCaching(builder);
        
        RegisterSettings(builder);

        RegisterDbContext(builder);

        RegisterJobs(builder);
    }
    
    // 注册日志
    private void RegisterLogger(ContainerBuilder builder)
    {
        builder.RegisterInstance(logger).AsSelf().AsImplementedInterfaces().SingleInstance();
    }
    
    // 注册中介者，用于解耦，后续调用 Service 请使用 IMediator
    private void RegisterMediator(ContainerBuilder builder)
    {
        var mediatorBuilder = new MediatorBuilder();

        mediatorBuilder.RegisterHandlers(_assemblies);
        mediatorBuilder.ConfigureGlobalReceivePipe(p =>
        {
            p.UseLogger();          // 统一日志记录
            p.UseMessageLogging();  // 特殊标记日志记录
            p.UseUnitOfWork();      // 事务处理
            p.UseUnifyResponse();   // 统一响应
        });

        builder.RegisterMediator(mediatorBuilder);
    }
    
    // 注册 AutoMapper，用于对象映射
    private void RegisterAutoMapper(ContainerBuilder builder)
    {
        builder.RegisterAutoMapper(false, _assemblies);
    }
    
    // 注册依赖注入
    private void RegisterDependency(ContainerBuilder builder)
    {
        var allServiceTypes = _assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && t.GetInterfaces().Any(i => i == typeof(IService)))
            .ToList();

        foreach (var type in allServiceTypes)
        {
            if (typeof(IScope).IsAssignableFrom(type))
                builder.RegisterType(type).AsImplementedInterfaces().InstancePerLifetimeScope();
            else if (typeof(ISingleton).IsAssignableFrom(type))
                builder.RegisterType(type).AsImplementedInterfaces().SingleInstance();
            else
                builder.RegisterType(type).AsImplementedInterfaces();
        }
    }
    
    // 注册缓存
    private void RegisterCaching(ContainerBuilder builder)
    {
        builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();
    }
    
    // 注册配置
    private void RegisterSettings(ContainerBuilder builder)
    {
        var settingTypes = _assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && typeof(IConfigurationSetting).IsAssignableFrom(t))
            .ToArray();

        builder.RegisterTypes(settingTypes).AsSelf().SingleInstance();
    }
    
    // 注册数据库上下文
    private void RegisterDbContext(ContainerBuilder builder)
    {
        builder.RegisterType<TemplateProjectDbContext>()
            .AsSelf()
            .As<DbContext>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterType<Repository>().As<IRepository>().InstancePerLifetimeScope();
    }
    
    // 注册任务
    private void RegisterJobs(ContainerBuilder builder)
    {
        foreach (var type in typeof(IJob).Assembly.GetTypes()
                     .Where(t => typeof(IJob).IsAssignableFrom(t) && t.IsClass))
        {
            builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}