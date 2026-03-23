using UserManagementApp.Modules.Auth.Infrastructure.Persistence;
using UserManagementApp.Modules.Shared.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

await AuthSeedData.SeedAsync(app.Services);

app.Run();