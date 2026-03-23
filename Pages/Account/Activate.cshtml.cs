using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementApp.Modules.Auth.Application.Models;
using UserManagementApp.Modules.Auth.Application.Services.Interfaces;

namespace UserManagementApp.Pages.Account;

public class ActivateModel : PageModel
{
    private readonly IAuthService _authService;

    public ActivateModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public ActivateAccountInputModel Input { get; set; } = new()
    {
        DocumentType = "DNI"
    };

    [TempData]
    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool IsSuccess { get; set; }

    public string ActivatedUserName { get; set; } = string.Empty;

    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(Input.DocumentType))
        {
            Input.DocumentType = "DNI";
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            IsSuccess = false;
            return Page();
        }

        var result = await _authService.ActivateAccountAsync(
            Input.DocumentType,
            Input.DocumentNumber,
            Input.Email,
            Input.PhoneNumber,
            Input.Password
        );

        if (!result.Success)
        {
            IsSuccess = false;
            StatusMessage = result.Message;
            return Page();
        }

        IsSuccess = true;
        StatusMessage = result.Message;
        ActivatedUserName = result.FirstName;

        return Page();
    }
}