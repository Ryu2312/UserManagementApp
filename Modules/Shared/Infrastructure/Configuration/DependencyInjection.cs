using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Modules.Auth.Application.Services;
using UserManagementApp.Modules.Auth.Application.Services.Interfaces;
using UserManagementApp.Modules.Auth.Infrastructure.Integrations;
using UserManagementApp.Modules.Shared.Infrastructure.Persistence;
using UserManagementApp.Modules.Users.Application.Services;
using UserManagementApp.Modules.Users.Application.Services.Interfaces;

namespace UserManagementApp.Modules.Shared.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorPages();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<DocumentApiOptions>(
            configuration.GetSection(DocumentApiOptions.SectionName));

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, FakeEmailService>();
        services.AddScoped<IUserProfileService, UserProfileService>();

        services.AddHttpClient<IDocumentLookupService, DocumentLookupService>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = true;
            });

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }
}