using UserManagementApp.Modules.Auth.Application.Services.Interfaces;

namespace UserManagementApp.Modules.Auth.Infrastructure.Integrations;

public class FakeEmailService : IEmailService
{
    public Task SendAccountBlockedEmailAsync(string toEmail, string fullName, DateTime? blockedUntil)
    {
        return Task.CompletedTask;
    }
}