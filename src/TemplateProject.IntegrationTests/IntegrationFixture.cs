using System.Collections.Concurrent;
using System.Text.Json.Nodes;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using NSubstitute;
using Respawn;
using Respawn.Graph;
using Serilog;
using TemplateProject.Core;
using TemplateProject.Core.Dbup;
using TemplateProject.Core.Settings;
using TemplateProject.Core.Settings.System;

namespace TemplateProject.IntegrationTest;

public class IntegrationFixture : IntegrationTestBase, IAsyncLifetime
{
    private readonly string _topic;
    private readonly string _databaseName;
    
    private static readonly ConcurrentDictionary<string, IContainer> Containers = new();
    private static readonly ConcurrentDictionary<string,bool> ShouldRunDbUpDatabases = new();
    
    private Respawner? _respawner;

    protected ILifetimeScope CurrentScope { get; }
    protected IConfiguration CurrentConfiguration => CurrentScope.Resolve<IConfiguration>();

    public IntegrationFixture(string topic, string databaseName)
    {
        _topic = topic;
        _databaseName = databaseName;
        
        var logger = Substitute.For<ILogger>();

        var container = Containers.GetValueOrDefault(_topic);
        if (container == null)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(l =>
            {
                l.AddSerilog();
            });
            
            var containerBuilder = new ContainerBuilder();
            
            var configuration = RegisterConfiguration(containerBuilder);
            
            containerBuilder.Populate(serviceCollection);
            containerBuilder.RegisterModule(new TemplateProjectModule(logger, configuration, typeof(IntegrationFixture).Assembly));
            containerBuilder.RegisterInstance(new MemoryCache(new MemoryCacheOptions())).AsSelf().As<IMemoryCache>().SingleInstance();
            
            container = containerBuilder.Build();
            Containers[_topic] = container;
        }
        
        CurrentScope = container.BeginLifetimeScope();
        
        RunDbUpIfRequired();
        SetupScope(CurrentScope);
    }

    private IConfiguration RegisterConfiguration(ContainerBuilder containerBuilder)
    {
        var targetJson = $"appsetting_{_topic}.json";
        File.Copy("appsettings.json", targetJson, true);

        var document = JsonNode.Parse(File.ReadAllText(targetJson)) ?? throw new InvalidOperationException();
        document["ConnectionStrings"]!["Mysql"] = document["ConnectionStrings"]!["Mysql"]!.ToString()
            .Replace("database=template_project", $"database=template_project_{_databaseName}");
        
        File.WriteAllText(targetJson, document.ToJsonString());
        
        var configuration = new ConfigurationBuilder().AddJsonFile(targetJson).Build();
        containerBuilder.RegisterInstance(configuration).AsImplementedInterfaces();
        
        configuration.Get(typeof(AppSetting));
        
        return configuration;
    }
    
    private void RunDbUpIfRequired()
    {
        if(!ShouldRunDbUpDatabases.GetValueOrDefault(_databaseName, true)) return;
        
        new DbUpRunner(new ConnectionStringSetting(CurrentConfiguration).Mysql).Run("Dbup");

        ShouldRunDbUpDatabases[_databaseName] = false;
    }
    
    public async Task InitializeAsync()
    {
        await using var conn = new MySqlConnection(new ConnectionStringSetting(CurrentConfiguration).Mysql);
        await conn.OpenAsync();
        
        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            TablesToIgnore = new Table[]
            {
                "schemaversions"
            },
            SchemasToInclude = new []
            {
                $"template_project_{_databaseName}"  
            },
            DbAdapter = DbAdapter.MySql
        });
    }

    public async Task DisposeAsync()
    {
        if (_respawner == null) return;
            
        await using var conn = new MySqlConnection(new ConnectionStringSetting(CurrentConfiguration).Mysql);
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
    }
}