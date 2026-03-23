using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagementApp.Pages.Account;

[Authorize]
public class KeepAliveModel : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        var claims = User.Claims.ToList();

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        HttpContext.Session.SetString("LastActivity", DateTime.UtcNow.ToString("O"));

        return new JsonResult(new { success = true });
    }
}