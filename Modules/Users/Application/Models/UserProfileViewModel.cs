namespace UserManagementApp.Modules.Users.Application.Models;

public class UserProfileViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? SecondaryEmail { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? SecondaryPhoneNumber { get; set; }
    public string? Nationality { get; set; }
    public string? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string EmploymentType { get; set; } = string.Empty;
    public DateTime HiringDate { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
}