using System.Data;
using System.Numerics;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restyrant.Services.Identity;
using Restyrant.Services.Identity.DbContexts;
using Restyrant.Services.Identity.Intilizer;
using Restyrant.Services.Identity.Models;
using Restyrant.Services.Identity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
var buidIdentityServer = builder.Services.AddIdentityServer(option =>
    {
        option.Events.RaiseErrorEvents = true;
        option.Events.RaiseInformationEvents = true;
        option.Events.RaiseFailureEvents = true;
        option.Events.RaiseSuccessEvents = true;
        option.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(SD.IdentityResources)
    .AddInMemoryApiScopes(SD.ApiScopes)
    .AddInMemoryClients(SD.Clients)
    .AddAspNetIdentity<ApplicationUser>();
builder.Services.AddScoped<IDbIntilizer, DbIntilizer>();
builder.Services.AddScoped<IProfileService, ProfileServices>();

buidIdentityServer.AddDeveloperSigningCredential();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
DataSeed();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
void DataSeed()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbIninitilezer = scope.ServiceProvider.GetRequiredService<IDbIntilizer>();
        dbIninitilezer.Intilizer();
    }
}