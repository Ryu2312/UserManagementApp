using UserManagementApp.Modules.Users.Application.Models;

namespace UserManagementApp.Modules.Users.Application.Services.Interfaces;

public interface IUserProfileService
{
    Task<UserProfileViewModel?> GetByUserIdAsync(int userId);
}