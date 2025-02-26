using System.Collections;

namespace TemplateProject.IntegrationTest.Managers.TestCase;

public class CacheManagerSetTestCase
{
    public required string Key { get; set; }
    
    public required Type Type { get; set; }
    
    public required object Data { get; set; }
}

public class CacheManagerSetDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new CacheManagerSetTestCase
            {
                Key = "String",
                Type = typeof(string),
                Data = "Test"
            }
        };
        
        yield return new object[]
        {
            new CacheManagerSetTestCase
            {
                Key = "List",
                Type = typeof(List<>),
                Data = new List<string>
                {
                    "123", "456", "789"
                }
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}