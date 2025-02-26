using Mediator.Net.Contracts;
using TemplateProject.Message.Dto;
using TemplateProject.Message.Dto.MessageLogging;

namespace TemplateProject.Message.Request.MessageLogging;

public class GetMessageLogPageRequest : PageInputDto, IRequest
{
    public List<Guid>? Ids { get; set; }

    public DateTimeOffset? StartDate { get; set; }

    public DateTimeOffset? EndDate { get; set; }
}

public class GetMessageLogPageResponse : TemplateProjectResponse<PagedListDto<MessageLogDto>>
{
}