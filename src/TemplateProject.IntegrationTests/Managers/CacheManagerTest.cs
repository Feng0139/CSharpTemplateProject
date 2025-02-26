using Shouldly;
using TemplateProject.Core.Caching;
using TemplateProject.IntegrationTest.Managers.TestCase;
using TemplateProject.IntegrationTest.TestBaseClasses;

namespace TemplateProject.IntegrationTest.Managers;

public class CacheManagerTest : ManagerFixtureBase
{
    [Theory]
    [ClassData(typeof(CacheManagerSetDataProvider))]
    public async Task ShouldSet(CacheManagerSetTestCase testCase)
    {
        await Run<ICacheManager>(async cache =>
        {
            await cache.Set(testCase.Key, testCase.Data);

            if (testCase.Type == typeof(string))
            {
                var value = await cache.Get<string>(testCase.Key);
                value.ShouldBeOfType(testCase.Type);
                value.ShouldBe(testCase.Data);
            }
            else if (testCase.Type == typeof(List<string>))
            {
                var value = await cache.Get<List<string>>(testCase.Key);
                value.ShouldBeOfType(testCase.Type);
                value.ShouldBe(testCase.Data);
            }
        });
    }
    
    [Fact]
    public async Task ShouldRemove()
    {
        await Run<ICacheManager>(async cache =>
        {
            await cache.Set("Test1", "TA");
            await cache.Remove("Test1");
            
            var value1 = await cache.Get<object>("Test1");
            
            value1.ShouldBeNull();
        });
    }
}