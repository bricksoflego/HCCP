//using BlueDragon.Data;
using BlueDragon.Services;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<BrandNameService>();
builder.Services.AddTransient<CableTypeService>();
builder.Services.AddTransient<CableService>();
builder.Services.AddTransient<EComponentService>();
builder.Services.AddTransient<HardwareService>();
builder.Services.AddTransient<PeripheralService>();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

