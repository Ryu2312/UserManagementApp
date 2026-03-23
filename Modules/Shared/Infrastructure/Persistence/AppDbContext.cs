using Microsoft.EntityFrameworkCore;
using UserManagementApp.Modules.Auth.Domain;
using UserManagementApp.Modules.Auth.Infrastructure.Persistence;

namespace UserManagementApp.Modules.Shared.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<AuthUser> Users => Set<AuthUser>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AuthUserConfiguration());
    }
}