using AutoMapper;
using TemplateProject.Core.Domain;
using TemplateProject.Core.Extension;
using TemplateProject.Message.Commands.MessageLogging;
using TemplateProject.Message.Dto;
using TemplateProject.Message.Dto.MessageLogging;

namespace TemplateProject.Core.Mappings;

public class MessageLogMapping : Profile
{
    public MessageLogMapping()
    {
        CreateMap<MessageLog, MessageLogDto>().ReverseMap();
        CreateMap<MessageLog, AddMessageLoggingCommand>().ReverseMap();
        
        CreateMap<PagedList<MessageLog>, PagedListDto<MessageLogDto>>().ReverseMap();
    }
}