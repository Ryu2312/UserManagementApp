using UserManagementApp.Modules.Auth.Application.Contracts;
using UserManagementApp.Modules.Auth.Domain;

namespace UserManagementApp.Modules.Auth.Application.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResult> ValidateLoginAsync(string documentType, string documentNumber, string password);

    Task<ActivateAccountResult> ActivateAccountAsync(
        string documentType,
        string documentNumber,
        string email,
        string phoneNumber,
        string password);

    Task SignInAsync(HttpContext httpContext, AuthUser user);
    Task SignOutAsync(HttpContext httpContext);
}