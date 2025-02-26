using Shouldly;
using TemplateProject.Core.Extension;

namespace TemplateProject.UnitTest.Encrypt;

public class CheckEncryptFixture
{
    [Theory]
    [InlineData("123456", "x2", "e10adc3949ba59abbe56e057f20f883e")]
    [InlineData("123456", "X2", "E10ADC3949BA59ABBE56E057F20F883E")]
    public void MD5(string source, string upperOrLower, string encryptResult)
    {
        var result = source.Md5Encrypt(upperOrLower);
        
        result.ShouldBe(encryptResult);
    }

    [Theory]
    [InlineData("SHA256", "123456", "x2", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92")]
    [InlineData("SHA384", "123456", "X2", "0A989EBC4A77B56A6E2BB7B19D995D185CE44090C13E2984B7ECC6D446D4B61EA9991B76A4C2F04B1B4D244841449454")]
    [InlineData("SHA512", "123456", "x2", "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413")]
    public void SHA(string algorithmName, string source, string upperOrLower, string encryptResult)
    {
        var result = source.SHAEncrypt(algorithmName, upperOrLower);
        
        result.ShouldBe(encryptResult);
    }
}