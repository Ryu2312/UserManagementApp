using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.Modules.Auth.Application.Models;
using UserManagementApp.Modules.Auth.Application.Services.Interfaces;

namespace UserManagementApp.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;

    public LoginModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; } = new()
    {
        DocumentType = "DNI"
    };

    [TempData]
    public string? StatusMessage { get; set; }

    public void OnGet(string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            StatusMessage = message;
        }

        if (string.IsNullOrWhiteSpace(Input.DocumentType))
        {
            Input.DocumentType = "DNI";
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _authService.ValidateLoginAsync(
            Input.DocumentType,
            Input.DocumentNumber,
            Input.Password
        );

        if (!result.Success)
        {
            if (result.User?.IsBlocked == true &&
                result.User.BlockedUntil.HasValue &&
                result.User.BlockedUntil.Value > DateTime.UtcNow)
            {
                return RedirectToPage("/Account/Blocked");
            }

            StatusMessage = result.Message;
            return Page();
        }

        await _authService.SignInAsync(HttpContext, result.User!);
        return RedirectToPage("/Users/Profile");
    }
}