using System.Text;

namespace TemplateProject.Core.Extension;

public static class StringExtension
{
    private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    public static string GenerateRandomString(this string prefix, int length = 9)
    {
        var random = new Random();
        var stringBuilder = new StringBuilder(length);

        for (var i = 0; i < length; i++)
        {
            var randomIndex = random.Next(chars.Length);
            stringBuilder.Append(chars[randomIndex]);
        }

        return prefix + stringBuilder.ToString();
    }
    
    public static string GenerateNumberSuffix(this string str, int length = 4)
    {
        var random = new Random();
        var suffix = random.Next(0, 10000);
        return suffix.ToString("D" + length);
    }
}