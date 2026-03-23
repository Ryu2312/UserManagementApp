using UserManagementApp.Modules.Auth.Domain;

namespace UserManagementApp.Modules.Auth.Application.Contracts;

public class LoginResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public AuthUser? User { get; set; }
}