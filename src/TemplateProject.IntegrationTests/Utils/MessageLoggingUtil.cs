using Autofac;
using TemplateProject.Core.Data;
using TemplateProject.Core.Domain;

namespace TemplateProject.IntegrationTest.Utils;

public class MessageLoggingUtil(ILifetimeScope lifetimeScope) : IntegrationUtilBase(lifetimeScope)
{
    public async Task AddMessageLoggingAsync(Guid id, string messageType, string messageJson, string resultType, string resultJson, DateTimeOffset createAt)
    {
        await RunWithUnitOfWork<IRepository>(async rep =>
        {
            await rep.InsertAsync(new MessageLog
            {
                Id = id,
                MessageType = messageType,
                MessageJson = messageJson,
                ResultType = resultType,
                ResultJson = resultJson,
                CreatedAt = createAt
            }).ConfigureAwait(false);
        });
    }
}