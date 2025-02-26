using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TemplateProject.Message.Dto;
using ILogger = Serilog.ILogger;

namespace TemplateProject.Api.ActionFilters;

public class UnifyResponseFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult { Value: not TemplateProjectResponse } objectResult)
        {
            context.Result = new JsonResult(new TemplateProjectResponse<object>
            {
                Data = objectResult.Value,
                Code = HttpStatusCode.OK,
                Message = nameof(HttpStatusCode.OK).ToLower()
            });
        }
        
        base.OnActionExecuted(context);
    }
}

public class UnifyResponseExceptionFilter : IExceptionFilter
{
    private readonly ILogger _logger;
    public UnifyResponseExceptionFilter(ILogger logger)
    {
        _logger = logger;
    }
    
    public void OnException(ExceptionContext context)
    {
        _logger.Warning("===== Exception {Action}: {@Exception}", context.ActionDescriptor.DisplayName, context.Exception);
        
        context.Result = new JsonResult(new TemplateProjectResponse<object>
        {
            Data = context.Result,
            Code = HttpStatusCode.InternalServerError,
            Message = context.Exception.Message
        });
        
        context.ExceptionHandled = true;
    }
}