using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using TemplateProject.Message.Request.MessageLogging;

namespace TemplateProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageLoggingController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("page")]
    public async Task<IActionResult> GetMessageLogPageAsync([FromQuery] GetMessageLogPageRequest request)
    {
        var response = await mediator.RequestAsync<GetMessageLogPageRequest, GetMessageLogPageResponse>(request).ConfigureAwait(false);
        
        return Ok(response);
    }
}