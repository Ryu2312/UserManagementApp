namespace UserManagementApp.Modules.Auth.Infrastructure.Integrations;

public class DocumentApiOptions
{
    public const string SectionName = "ExternalApis:DocumentApi";

    public string BaseUrl2 { get; set; } = string.Empty;
    public string Token2 { get; set; } = string.Empty;
}