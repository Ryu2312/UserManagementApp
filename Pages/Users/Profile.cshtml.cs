using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.Modules.Users.Application.Models;
using UserManagementApp.Modules.Users.Application.Services.Interfaces;

namespace UserManagementApp.Pages.Users;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IUserProfileService _userProfileService;

    public ProfileModel(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    public UserProfileViewModel? Profile { get; set; }

    public async Task OnGetAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(userIdClaim, out var userId))
        {
            Profile = await _userProfileService.GetByUserIdAsync(userId);
        }
    }
}