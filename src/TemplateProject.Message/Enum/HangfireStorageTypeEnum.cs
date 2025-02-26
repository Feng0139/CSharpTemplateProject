using System.ComponentModel;

namespace TemplateProject.Message.Enum;

public enum HangfireStorageTypeEnum
{
    [Description("Memory")]
    Memory = 0,
    
    [Description("Redis")]
    Redis = 1
}