using UserManagementApp.Modules.Auth.Application.Services.Interfaces;

namespace UserManagementApp.Modules.Auth.Infrastructure.Integrations;

public class FakeEmailService : IEmailService
{
    private readonly ILogger<FakeEmailService> _logger;

    public FakeEmailService(ILogger<FakeEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAccountBlockedEmailAsync(string toEmail, string fullName, DateTime? blockedUntil)
    {
        _logger.LogInformation(
            "Correo de bloqueo enviado a {Email} para {FullName}. Bloqueado hasta {BlockedUntil}",
            toEmail,
            fullName,
            blockedUntil
        );

        return Task.CompletedTask;
    }
}