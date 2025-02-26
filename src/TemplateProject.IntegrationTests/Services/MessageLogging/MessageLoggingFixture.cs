using Mediator.Net;
using Shouldly;
using TemplateProject.IntegrationTest.Services.MessageLogging.TestClass;
using TemplateProject.IntegrationTest.TestBaseClasses;
using TemplateProject.IntegrationTest.Utils;
using TemplateProject.Message.Request.MessageLogging;

namespace TemplateProject.IntegrationTest.Services.MessageLogging;

public class MessageLoggingFixture : MessageLoggingFixtureBase
{
    private readonly MessageLoggingUtil _util;
    
    public MessageLoggingFixture()
    {
        _util = new MessageLoggingUtil(CurrentScope);
    }
    
    [Theory]
    [ClassData(typeof(GetMessageLoggingDataProvider))]
    public async Task GetLogPage(GetMessageLoggingTestCase testCase)
    {
        await _util.AddMessageLoggingAsync(Guid.Parse("fb46b581-7837-41ca-b734-db32ffd9c7e9"), "string", "{}", "string", "{}", DateTimeOffset.Parse("2024-08-05"));
        await _util.AddMessageLoggingAsync(Guid.Parse("ffee8b77-40d8-4dfd-8a7a-4b4cf936ab35"), "string", "{}", "string", "{}", DateTimeOffset.Parse("2024-08-09"));
        await _util.AddMessageLoggingAsync(Guid.Parse("ae668eb7-9922-4469-8c0b-1c24c4fa0160"), "string", "{}", "string", "{}", DateTimeOffset.Parse("2024-08-25"));
        await _util.AddMessageLoggingAsync(Guid.NewGuid(), "int", "1", "string", "{}", DateTimeOffset.Parse("2024-08-05"));
        await _util.AddMessageLoggingAsync(Guid.NewGuid(), "double", "3", "string", "{}", DateTimeOffset.Parse("2024-08-09"));
        await _util.AddMessageLoggingAsync(Guid.NewGuid(), "int", "2", "string", "{}", DateTimeOffset.Parse("2024-08-25"));
        await _util.AddMessageLoggingAsync(Guid.NewGuid(), "double", "4", "string", "{}", DateTimeOffset.Parse("2024-08-26"));
        
        await Run<IMediator>(async mediator =>
        {
            var response = await mediator.RequestAsync<GetMessageLogPageRequest, GetMessageLogPageResponse>(new GetMessageLogPageRequest
            {
                Ids = testCase.Ids,
                StartDate = testCase.StartDate,
                EndDate = testCase.EndDate,
                PageIndex = testCase.PageIndex,
                PageSize = testCase.PageSize
            });

            response.Data.ShouldNotBeNull();
            
            response.Data.Items.Count().ShouldBe(testCase.SuccessesCount);
        });
    }
}