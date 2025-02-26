using Autofac;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.Redis.StackExchange;
using Serilog;
using TemplateProject.Api.ActionFilters;
using TemplateProject.Core;
using TemplateProject.Core.Extension;
using TemplateProject.Core.Settings.System;
using TemplateProject.Message.Enum;

namespace TemplateProject.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    private readonly HangfireSetting _hangfireSetting;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        _hangfireSetting = new HangfireSetting(_configuration);
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers(options =>
        {
            options.Filters.Add<UnifyResponseFilter>();
            options.Filters.Add<UnifyResponseExceptionFilter>();
        });
        
        services.AddHangfire(configuration =>
        {
            switch(_hangfireSetting.UseStorage)
            {
                case HangfireStorageTypeEnum.Redis:
                    configuration.UseRedisStorage(new ConnectionStringSetting(_configuration).Redis);
                    break;
                case HangfireStorageTypeEnum.Memory:
                default:
                    configuration.UseMemoryStorage();
                    break;
            }
        });
        services.AddHangfireServer();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new TemplateProjectModule(Log.Logger, _configuration, typeof(TemplateProjectModule).Assembly));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();

            app.UseStaticFiles();
            
            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-dark-theme.css");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(ep => ep.MapControllers());
        
        if (_hangfireSetting.EnableDashboard)
        {
            app.UseHangfireDashboard();
        }
        app.UseForgetJobs();
        app.UseDelayedJobs();
        app.UseRecurringJobs();
    }
}