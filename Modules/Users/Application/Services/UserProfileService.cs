using Microsoft.EntityFrameworkCore;
using UserManagementApp.Modules.Shared.Infrastructure.Persistence;
using UserManagementApp.Modules.Users.Application.Models;
using UserManagementApp.Modules.Users.Application.Services.Interfaces;

namespace UserManagementApp.Modules.Users.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly AppDbContext _dbContext;

    public UserProfileService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProfileViewModel?> GetByUserIdAsync(int userId)
    {
        return await _dbContext.Users
            .Where(x => x.Id == userId)
            .Select(x => new UserProfileViewModel
            {
                FullName = x.FullName,
                DocumentType = x.DocumentType,
                DocumentNumber = x.DocumentNumber,
                Email = x.Email,
                SecondaryEmail = x.SecondaryEmail,
                PhoneNumber = x.PhoneNumber,
                SecondaryPhoneNumber = x.SecondaryPhoneNumber,
                Nationality = x.Nationality,
                Gender = x.Gender,
                BirthDate = x.BirthDate,
                EmploymentType = x.EmploymentType,
                HiringDate = x.HiringDate,
                LastLoginAt = x.LastLoginAt,
                ActivatedAt = x.ActivatedAt
            })
            .FirstOrDefaultAsync();
    }
}