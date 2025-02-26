using System.Reflection;
using System.Text.Json.Serialization;

namespace TemplateProject.Core.Extension;

public static class ObjectExtension
{
    public static IEnumerable<T> WhereIF<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        return condition ? source.Where(predicate) : Enumerable.Empty<T>();
    }
    
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
    {
        return source == null || !source.Any();
    }
    
    public static string GetGenericTypeName(this Type type)
    {
        string typeName;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }
    
    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
    
    public static Dictionary<string, string> ToDictionary(this object source)
    {
        var dictionary = new Dictionary<string, string>();
        var properties = source.GetType().GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(source);
            if (value == null) continue;
            
            var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            var key = jsonPropertyAttribute != null ? jsonPropertyAttribute.Name : property.Name;
            dictionary.Add(key, value.ToString()!);
        }

        return dictionary;
    }
    
    public static T ToEnum<T>(this object value)
    {
        return (T)Enum.ToObject(typeof(T), value);
    }
}