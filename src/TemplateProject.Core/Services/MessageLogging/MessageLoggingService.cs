using AutoMapper;
using TemplateProject.Core.Domain;
using TemplateProject.Message.Commands.MessageLogging;
using TemplateProject.Message.Dto;
using TemplateProject.Message.Dto.MessageLogging;
using TemplateProject.Message.Request.MessageLogging;

namespace TemplateProject.Core.Services.MessageLogging;

public interface IMessageLoggingService : IScope
{
    Task<GetMessageLogPageResponse> GetLogPageAsync(GetMessageLogPageRequest request, CancellationToken cancellationToken);
    
    Task AddLogsAsync(List<AddMessageLoggingCommand> commands, CancellationToken cancellationToken);
}
    
public class MessageLoggingService(IMapper mapper, IMessageLoggingDataProvider dataProvider) : IMessageLoggingService
{
    public async Task<GetMessageLogPageResponse> GetLogPageAsync(GetMessageLogPageRequest request, CancellationToken cancellationToken)
    {
        var paged = await dataProvider.GetFilterAsync(
            request.Ids,
            request.StartDate, request.EndDate,
            request.PageIndex, request.PageSize, cancellationToken).ConfigureAwait(false);

        return new GetMessageLogPageResponse
        {
            Data = mapper.Map<PagedListDto<MessageLogDto>>(paged)
        };
    }


    public async Task AddLogsAsync(List<AddMessageLoggingCommand> commands, CancellationToken cancellationToken)
    {
        var logs = mapper.Map<List<MessageLog>>(commands);
        
        await dataProvider.AddMessageLogsAsync(logs, cancellationToken).ConfigureAwait(false);
    }
}