using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Modules.Auth.Application.Contracts;
using UserManagementApp.Modules.Auth.Application.Services.Interfaces;
using UserManagementApp.Modules.Auth.Domain;
using UserManagementApp.Modules.Shared.Infrastructure.Persistence;
using System.Globalization;

namespace UserManagementApp.Modules.Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly IDocumentLookupService _documentLookupService;
    public AuthService(
        AppDbContext dbContext,
        IPasswordService passwordService,
        IEmailService emailService,
        IDocumentLookupService documentLookupService
        )
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
        _emailService = emailService;
        _documentLookupService = documentLookupService;
    }

    public async Task<LoginResult> ValidateLoginAsync(string documentType, string documentNumber, string password)
    {
        documentType = documentType.Trim().ToUpperInvariant();
        documentNumber = documentNumber.Trim().ToUpperInvariant();

        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            x.DocumentType.Trim().ToUpper() == documentType &&
            x.DocumentNumber.Trim().ToUpper() == documentNumber);

        if (user is null)
        {
            return new LoginResult
            {
                Success = false,
                Message = "No se encontró un usuario con ese documento."
            };
        }

        if (user.IsBlocked && user.BlockedUntil.HasValue && user.BlockedUntil > DateTime.UtcNow)
        {
            return new LoginResult
            {
                Success = false,
                Message = "La cuenta se encuentra bloqueada temporalmente.",
                User = user
            };
        }

        if (!user.IsActive)
        {
            return new LoginResult
            {
                Success = false,
                Message = "La cuenta aún no ha sido activada.",
                User = user
            };
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash) || !_passwordService.VerifyPassword(password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= 3)
            {
                user.IsBlocked = true;
                user.BlockedUntil = DateTime.UtcNow.AddMinutes(5);

                await _emailService.SendAccountBlockedEmailAsync(
                    user.Email,
                    user.FullName,
                    user.BlockedUntil
                );
            }

            await _dbContext.SaveChangesAsync();

            return new LoginResult
            {
                Success = false,
                Message = "Credenciales inválidas.",
                User = user
            };
        }

        user.FailedLoginAttempts = 0;
        user.IsBlocked = false;
        user.BlockedUntil = null;
        user.LastLoginAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new LoginResult
        {
            Success = true,
            Message = "Inicio de sesión exitoso.",
            User = user
        };
    }

    public async Task<ActivateAccountResult> ActivateAccountAsync(
    string documentType,
    string documentNumber,
    string email,
    string phoneNumber,
    string password)
    {
        documentType = documentType.Trim().ToUpperInvariant();
        documentNumber = documentNumber.Trim().ToUpperInvariant();
        email = email.Trim();
        phoneNumber = phoneNumber.Trim();

        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            x.DocumentType == documentType &&
            x.DocumentNumber == documentNumber);

        var documentData = await _documentLookupService.GetPersonAsync(documentType, documentNumber);

        if (documentData is null)
        {
            return new ActivateAccountResult
            {
                Success = false,
                Message = "No se encontró información para el documento ingresado."
            };
        }

        if (user is not null && user.IsActive)
        {
            return new ActivateAccountResult
            {
                Success = false,
                Message = "La cuenta ya fue activada.",
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        if (user is null)
        {
            user = new AuthUser
            {
                DocumentType = documentType,
                DocumentNumber = documentNumber,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
        }

        user.FirstName = documentData.FirstName;
        user.LastName = documentData.LastName;
        user.Email = email;
        user.PhoneNumber = phoneNumber;
        user.SecondaryEmail = null;
        user.SecondaryPhoneNumber = null;
        user.Nationality = documentData.Nationality;
        user.Gender = documentData.Gender;
        var parsedBirthDate = ParseBirthDate(documentData.BirthDate);
        user.BirthDate = parsedBirthDate;
        user.PasswordHash = _passwordService.HashPassword(password);
        user.IsActive = true;
        user.ActivatedAt = DateTime.UtcNow;
        user.EmploymentType = string.IsNullOrWhiteSpace(user.EmploymentType)
            ? GetRandomEmploymentType()
            : user.EmploymentType;
        user.HiringDate = user.ActivatedAt ?? user.CreatedAt;

        await _dbContext.SaveChangesAsync();

        return new ActivateAccountResult
        {
            Success = true,
            Message = "Cuenta activada correctamente.",
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task SignInAsync(HttpContext httpContext, AuthUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("DocumentType", user.DocumentType),
            new Claim("DocumentNumber", user.DocumentNumber)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    public async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private static readonly string[] EmploymentTypes =
{
    "CAS",
    "DL 276",
    "DL 728",
    "Locación de servicios"
};

    private static string GetRandomEmploymentType()
    {
        var index = Random.Shared.Next(EmploymentTypes.Length);
        return EmploymentTypes[index];
    }


    private static DateTime? ParseBirthDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var formats = new[]
        {
        "dd/MM/yyyy",     // 👈 el tuyo actual
        "yyyy-MM-dd",
        "yyyy-MM-ddTHH:mm:ss"
    };

        if (DateTime.TryParseExact(
            value,
            formats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var parsed))
        {
            return parsed;
        }

        return null;
    }
}