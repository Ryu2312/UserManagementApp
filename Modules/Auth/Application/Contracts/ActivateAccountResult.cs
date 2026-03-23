namespace UserManagementApp.Modules.Auth.Application.Contracts;

public class ActivateAccountResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string FullName =>
        string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
}