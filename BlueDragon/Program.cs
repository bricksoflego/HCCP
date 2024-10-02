using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
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

var sqlPassword = builder.Configuration["SQLPassword"];
var connectionString = builder?.Configuration?.GetConnectionString("SQLServer")?.Replace("{SQLPassword}", sqlPassword);
builder!.Services.AddDbContext<HccContext>(options => options.UseSqlServer(connectionString));
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
builder.Services.AddScoped<IBrandNameService, BrandNameService>();
builder.Services.AddScoped<ICableTypeService, CableTypeService>();
builder.Services.AddScoped<ICableService, CableService>();
builder.Services.AddScoped<IEComponentService, EComponentService>();
builder.Services.AddScoped<IHardwareService, HardwareService>();
builder.Services.AddScoped<IPeripheralService, PeripheralService>();
builder.Services.AddScoped<ISolutionService, SolutionService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IFormFieldClearService, FormFieldClearService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<ApplicationUserService>();
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
