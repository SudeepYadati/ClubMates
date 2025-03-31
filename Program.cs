using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ClubMates.Web.Models;
using ClubMates.Web.Controllers.AppDbcontext;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ClubMatesUser, IdentityRole>(Options =>
    {
    Options.Password.RequireDigit = true;
    Options.Password.RequireLowercase = true;
    Options.Password.RequireUppercase = true;
    Options.Password.RequiredLength = 6;
        Options.Lockout= new LockoutOptions()
        {
            AllowedForNewUsers = true,
            DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
            MaxFailedAccessAttempts = 5
        };
        Options.User.RequireUniqueEmail = true;
    }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeASuperAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin").RequireClaim(ClaimTypes.Role, "Guest"));
    options.AddPolicy("MustBeAGuest", policy => policy.RequireClaim(ClaimTypes.Role, "Guest"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
