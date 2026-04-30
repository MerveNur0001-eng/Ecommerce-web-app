using System.Security.Claims;
using Ecommerce.Data;
using Ecommerce.Service.Abstract;
using Ecommerce.Service.Concrete;
using Ecommerce.WebUI.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Ecommerce.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromDays(1);
});

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<MailHelper>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.LoginPath = "/Account/SignIn";
        x.AccessDeniedPath = "/AccessDenied";
        x.Cookie.Name = "Account";
        x.Cookie.MaxAge = TimeSpan.FromDays(7);
        x.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));

    x.AddPolicy("UserPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin", "User", "Customer"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();