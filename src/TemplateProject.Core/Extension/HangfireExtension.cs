using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TemplateProject.Core.Jobs;

namespace TemplateProject.Core.Extension;

public static class HangfireExtension
{
    public static void UseForgetJobs(this IApplicationBuilder app)
    {
        foreach (var type in typeof(IForgetJob).Assembly.GetTypes().Where(type => type.IsClass && typeof(IForgetJob).IsAssignableFrom(type)))
        {
            var job = (IForgetJob)app.ApplicationServices.GetRequiredService(type);

            if (typeof(IForgetJob).IsAssignableFrom(type))
            {
                BackgroundJob.Enqueue(() => job.Execute());
            }
        }
    }
    
    public static void UseDelayedJobs(this IApplicationBuilder app)
    {
        foreach (var type in typeof(IDelayedJob).Assembly.GetTypes().Where(type => type.IsClass && typeof(IDelayedJob).IsAssignableFrom(type)))
        {
            var job = (IDelayedJob)app.ApplicationServices.GetRequiredService(type);

            if (typeof(IDelayedJob).IsAssignableFrom(type))
            {
                BackgroundJob.Schedule(() => job.Execute(), job.Delay);
            }
        }
    }
    
    public static void UseRecurringJobs(this IApplicationBuilder app)
    {
        foreach (var type in typeof(IRecurringJob).Assembly.GetTypes().Where(type => type.IsClass && typeof(IRecurringJob).IsAssignableFrom(type)))
        {
            var job = (IRecurringJob)app.ApplicationServices.GetRequiredService(type);

            if (typeof(IRecurringJob).IsAssignableFrom(type))
            {
                RecurringJob.AddOrUpdate(job.JobId, () => job.Execute(), job.CronExpression);
            }
        }
    }
}