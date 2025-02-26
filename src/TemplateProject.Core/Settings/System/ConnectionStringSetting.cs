using Microsoft.Extensions.Configuration;

namespace TemplateProject.Core.Settings.System;

public class ConnectionStringSetting : IConfigurationSetting
{
    public string Mysql { get; }
    
    public string Redis { get; }
    
    public ConnectionStringSetting(IConfiguration configuration)
    {
        Mysql = configuration["ConnectionStrings:Mysql"] ?? string.Empty;
        Redis = configuration["ConnectionStrings:Redis"] ?? string.Empty;
    }
}