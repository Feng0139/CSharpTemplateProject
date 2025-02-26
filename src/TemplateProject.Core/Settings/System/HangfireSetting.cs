using Microsoft.Extensions.Configuration;
using TemplateProject.Message.Enum;

namespace TemplateProject.Core.Settings.System;

public class HangfireSetting : IConfigurationSetting
{
    public bool EnableDashboard { get; }
    
    public HangfireStorageTypeEnum UseStorage { get; }

    public HangfireSetting(IConfiguration configuration)
    {
        EnableDashboard = configuration.GetValue<bool>("Hangfire:EnableDashboard");
        
        UseStorage = configuration.GetValue<HangfireStorageTypeEnum>("Hangfire:UseStorage");
    }
}