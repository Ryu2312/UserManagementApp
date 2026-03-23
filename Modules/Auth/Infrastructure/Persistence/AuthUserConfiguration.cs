using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementApp.Modules.Auth.Domain;

namespace UserManagementApp.Modules.Auth.Infrastructure.Persistence;

public class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
{
    public void Configure(EntityTypeBuilder<AuthUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.DocumentType, x.DocumentNumber })
            .IsUnique();

        builder.Property(x => x.DocumentType)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(x => x.DocumentNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.SecondaryEmail)
            .HasMaxLength(150);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.SecondaryPhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.EmploymentType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Nationality)
            .HasMaxLength(100);

        builder.Property(x => x.Gender)
            .HasMaxLength(30);

        builder.Property(x => x.BirthDate);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(false);

        builder.Property(x => x.IsBlocked)
            .HasDefaultValue(false);

        builder.Property(x => x.FailedLoginAttempts)
            .HasDefaultValue(0);
    }
}