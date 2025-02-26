using System.Security.Cryptography;
using System.Text;

namespace TemplateProject.Core.Extension;

public static class CryptogramExtension
{
    /// <summary>
    /// MD5加密, 仅做校验文件完整性.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="upperOrLower">十六进制: "x2" 小写, "X2" 大写</param>
    /// <returns></returns>
    public static string Md5Encrypt(this string source, string upperOrLower = "x2")
    {
        var tmpByte = Encoding.UTF8.GetBytes(source);
        var result = MD5.HashData(tmpByte);
        
        var stringBuilder = new StringBuilder();
        foreach (var t in result)
        {
            stringBuilder.Append(t.ToString(upperOrLower));
        }
        
        return stringBuilder.ToString();
    }

    /// <summary>
    /// SHA加密
    /// </summary>
    /// <param name="source"></param>
    /// <param name="algorithmName">SHA变种: SHA256, SHA384, SHA512</param>
    /// <param name="upperOrLower">十六进制: "x2" 小写, "X2" 大写</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string SHAEncrypt(this string source, string algorithmName = "SHA256", string upperOrLower = "x2")
    {
        HashAlgorithm algorithm = algorithmName switch
        {
            "SHA256" => SHA256.Create(),
            "SHA384" => SHA384.Create(),
            "SHA512" => SHA512.Create(),
            _ => throw new ArgumentException("Invalid algorithm name", nameof(algorithmName))
        };

        var bytes = Encoding.UTF8.GetBytes(source);
        var hash = algorithm.ComputeHash(bytes);

        var stringBuilder = new StringBuilder();
        foreach (var t in hash)
        {
            stringBuilder.Append(t.ToString(upperOrLower));
        }

        return stringBuilder.ToString();
    }
}