namespace UserManagementApp.Modules.Auth.Application.Models;

public class DocumentLookupResult
{
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string FatherLastName { get; set; } = string.Empty;
    public string MotherLastName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;
    public string Nationality { get; set; } = "Peruana";
}