using Serilog;

namespace TemplateProject.Core.Jobs.RecurringJobs;

public class DailyMessageJob(ILogger logger) : IRecurringJob
{
    public string JobId => nameof(DailyMessageJob);

    public string CronExpression => "0 0 1 ? * * ";

    public Task Execute()
    {
        logger.Warning($"=== Now Time {DateTime.Now}" );
        
        return Task.CompletedTask;
    }
}