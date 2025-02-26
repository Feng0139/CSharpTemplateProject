using System.Reflection;
using DbUp;
using DbUp.ScriptProviders;

namespace TemplateProject.Core.Dbup;

public class DbUpRunner(string connectionString)
{
    public void Run(string fileScriptPath)
    {
        EnsureDatabase.For.MySqlDatabase(connectionString);

        var loadScriptDirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, fileScriptPath);
        var filesystemScriptOptions = new FileSystemScriptOptions
        {
            IncludeSubDirectories = true,
        };

        var upgradeEngine = DeployChanges.To
            .MySqlDatabase(connectionString)
            .WithScriptsFromFileSystem(loadScriptDirPath, filesystemScriptOptions)
            .WithTransaction()
            .WithExecutionTimeout(TimeSpan.FromMinutes(3))
            .LogToConsole()
            .Build();
        
        var databaseUpgradeResult = upgradeEngine.PerformUpgrade();
        if (!databaseUpgradeResult.Successful)
        {
            throw new Exception("DbUp failed", databaseUpgradeResult.Error);
        }
    }
}