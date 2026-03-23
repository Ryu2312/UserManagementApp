using Microsoft.EntityFrameworkCore;
using UserManagementApp.Modules.Auth.Application.Services.Interfaces;
using UserManagementApp.Modules.Auth.Domain;
using UserManagementApp.Modules.Shared.Infrastructure.Persistence;

namespace UserManagementApp.Modules.Auth.Infrastructure.Persistence;

public static class AuthSeedData
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        await dbContext.Database.MigrateAsync();

        await UpsertUserAsync(
            dbContext,
            new AuthUser
            {
                DocumentType = "DNI",
                DocumentNumber = "12345678",
                FirstName = "July Camila",
                LastName = "Mendoza Quispe",
                Email = "july@test.com",
                SecondaryEmail = null,
                PhoneNumber = "999999999",
                SecondaryPhoneNumber = null,
                Nationality = "Peruana",
                Gender = "Femenino",
                BirthDate = new DateTime(1998, 4, 15),
                EmploymentType = "CAS",
                HiringDate = new DateTime(2025, 3, 9),
                PasswordHash = null,
                IsActive = false,
                IsBlocked = false,
                FailedLoginAttempts = 0,
                BlockedUntil = null,
                CreatedAt = DateTime.UtcNow,
                ActivatedAt = null,
                LastLoginAt = null
            },
            preservePassword: true);

        await UpsertUserAsync(
            dbContext,
            new AuthUser
            {
                DocumentType = "CE",
                DocumentNumber = "X1234567",
                FirstName = "Adriana",
                LastName = "Osorio Montes",
                Email = "adriana@test.com",
                SecondaryEmail = null,
                PhoneNumber = "988777666",
                SecondaryPhoneNumber = null,
                Nationality = "Peruana",
                Gender = "Femenino",
                BirthDate = new DateTime(1994, 7, 20),
                EmploymentType = "CAS",
                HiringDate = new DateTime(2025, 3, 9),
                PasswordHash = passwordService.HashPassword("Admin123*"),
                IsActive = true,
                IsBlocked = false,
                FailedLoginAttempts = 0,
                BlockedUntil = null,
                CreatedAt = DateTime.UtcNow,
                ActivatedAt = DateTime.UtcNow,
                LastLoginAt = null
            },
            preservePassword: false);

        await dbContext.SaveChangesAsync();
    }

    private static async Task UpsertUserAsync(
        AppDbContext dbContext,
        AuthUser seedUser,
        bool preservePassword)
    {
        var existingUser = await dbContext.Users.FirstOrDefaultAsync(x =>
            x.DocumentType == seedUser.DocumentType &&
            x.DocumentNumber == seedUser.DocumentNumber);

        if (existingUser is null)
        {
            dbContext.Users.Add(seedUser);
            return;
        }

        existingUser.FirstName = seedUser.FirstName;
        existingUser.LastName = seedUser.LastName;
        existingUser.Email = seedUser.Email;
        existingUser.SecondaryEmail = seedUser.SecondaryEmail;
        existingUser.PhoneNumber = seedUser.PhoneNumber;
        existingUser.SecondaryPhoneNumber = seedUser.SecondaryPhoneNumber;
        existingUser.Nationality = seedUser.Nationality;
        existingUser.Gender = seedUser.Gender;
        existingUser.BirthDate = seedUser.BirthDate;
        existingUser.EmploymentType = seedUser.EmploymentType;
        existingUser.HiringDate = seedUser.HiringDate;
        existingUser.IsBlocked = false;
        existingUser.FailedLoginAttempts = 0;
        existingUser.BlockedUntil = null;

        if (!preservePassword)
        {
            existingUser.PasswordHash = seedUser.PasswordHash;
            existingUser.IsActive = seedUser.IsActive;
            existingUser.ActivatedAt = seedUser.ActivatedAt;
        }

        if (existingUser.CreatedAt == default)
        {
            existingUser.CreatedAt = seedUser.CreatedAt;
        }
    }
}