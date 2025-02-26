using Mediator.Net.Context;
using Mediator.Net.Contracts;
using TemplateProject.Core.Services.MessageLogging;
using TemplateProject.Message.Request.MessageLogging;

namespace TemplateProject.Core.Handler.RequestHandler.MessageLogging;

public class GetMessageLogPageHandler : IRequestHandler<GetMessageLogPageRequest, GetMessageLogPageResponse>
{
    private readonly IMessageLoggingService _service;

    public GetMessageLogPageHandler(IMessageLoggingService service)
    {
        _service = service;
    }
    
    public async Task<GetMessageLogPageResponse> Handle(IReceiveContext<GetMessageLogPageRequest> context, CancellationToken cancellationToken)
    {
        return await _service.GetLogPageAsync(context.Message, cancellationToken).ConfigureAwait(false);
    }
}