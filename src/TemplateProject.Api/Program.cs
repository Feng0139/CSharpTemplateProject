using Autofac.Extensions.DependencyInjection;
using Serilog;
using TemplateProject.Core.Dbup;
using TemplateProject.Core.Settings;
using TemplateProject.Core.Settings.System;

namespace TemplateProject.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        configuration.Get(typeof(AppSetting));
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithProperty("ReleaseVersion", configuration.GetValue<string>("ReleaseVersion"))
            .CreateLogger();
        
        var connectionSetting = new ConnectionStringSetting(configuration);
        new DbUpRunner(connectionSetting.Mysql).Run(nameof(Core.Dbup));
        
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureLogging(l => l.AddSerilog(Log.Logger))
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build()
            .Run();
    }
}