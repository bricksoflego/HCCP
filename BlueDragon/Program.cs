using BlueDragon;
using BlueDragon.Components;
using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using BlueDragon.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("SQLServer");
builder.Services.AddDbContext<HccContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HccContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("MustBeAdmin", policy => policy.RequireRole("Admin"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<BrandNameService>();
builder.Services.AddTransient<CableTypeService>();
builder.Services.AddTransient<CableService>();
builder.Services.AddTransient<EComponentService>();
builder.Services.AddTransient<HardwareService>();
builder.Services.AddTransient<PeripheralService>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddTransient<SolutionService>();
builder.Services.AddSingleton<AuditStateService>();

// Register the CustomAuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<ApplicationUserService>();

// Register AuthService after CustomAuthenticationStateProvider
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddMudServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await InitializeRoles(roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

static async Task InitializeRoles(RoleManager<IdentityRole> roleManager)
{
    var roles = new List<string> { "Admin", "User", "Manager" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
