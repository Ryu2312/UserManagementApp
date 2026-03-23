using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.Modules.Auth.Application.Services.Interfaces;

namespace UserManagementApp.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly IAuthService _authService;

    public LogoutModel(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _authService.SignOutAsync(HttpContext);
        return RedirectToPage("/Account/Login", new { message = "Sesión cerrada correctamente" });
    }

    public IActionResult OnGet()
    {
        return RedirectToPage("/Account/Login");
    }
}