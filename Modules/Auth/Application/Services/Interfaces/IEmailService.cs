namespace UserManagementApp.Modules.Auth.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendAccountBlockedEmailAsync(string toEmail, string fullName, DateTime? blockedUntil);
}