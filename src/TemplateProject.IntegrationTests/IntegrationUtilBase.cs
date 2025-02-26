using Autofac;

namespace TemplateProject.IntegrationTest;

public class IntegrationUtilBase : IntegrationTestBase
{
    public IntegrationUtilBase(ILifetimeScope lifetimeScope)
    {
        SetupScope(lifetimeScope);
    }
}