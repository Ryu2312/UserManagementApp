using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Modules.Auth.Domain;

public class AuthUser
{
    public int Id { get; set; }

    [Required]
    [MaxLength(5)]
    public string DocumentType { get; set; } = "DNI";

    [Required]
    [MaxLength(20)]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? SecondaryEmail { get; set; }

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? SecondaryPhoneNumber { get; set; }

    [MaxLength(100)]
    public string? Nationality { get; set; }

    [MaxLength(30)]
    public string? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string EmploymentType { get; set; } = string.Empty;

    public DateTime HiringDate { get; set; }

    [MaxLength(500)]
    public string? PasswordHash { get; set; }

    public bool IsActive { get; set; } = false;
    public bool IsBlocked { get; set; } = false;
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? BlockedUntil { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ActivatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
}