using System.Text.Json;

namespace TemplateProject.Core.Extension;

public static class SerializationExtension
{
    public static string SerializeJson(this object source)
    {
        return JsonSerializer.Serialize(source);
    }
    
    public static T DeserializeJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}