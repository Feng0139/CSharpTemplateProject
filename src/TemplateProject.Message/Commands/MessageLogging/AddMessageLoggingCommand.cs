using Mediator.Net.Contracts;

namespace TemplateProject.Message.Commands.MessageLogging;

public class AddMessageLoggingCommand : ICommand
{
    public string MessageType { get; set; } = string.Empty;
    
    public string ResultType { get; set; } = string.Empty;
    
    public string MessageJson { get; set; } = string.Empty;
    
    public string ResultJson { get; set; } = string.Empty;
}