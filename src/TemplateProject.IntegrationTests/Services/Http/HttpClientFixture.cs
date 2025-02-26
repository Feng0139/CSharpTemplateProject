using Shouldly;
using TemplateProject.Core.Services.Http;

namespace TemplateProject.IntegrationTest.Services.Http;

public class HttpClientFixture() : IntegrationFixture("httpclient", "httpclient_test")
{
    [Fact]
    public async Task RequestAsync()
    {
        await Run<ITemplateProjectHttpClientFactory>(async factory =>
        {
            var response = await factory.GetAsync<string>("http://www.baidu.com", default);

            response.Data.ShouldNotBeNull();
        });
    }
}