namespace TemplateProject.Message.Dto.HttpClient;

public class TokenModel
{
    public string Scheme { get; set; } = "Bearer";
    
    public string Parameter { get; set; } = string.Empty;
    
    public string Token => $"{Scheme} {Parameter}";
}