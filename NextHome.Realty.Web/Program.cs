using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;
using NextHome.Realty.Persistence.Repository;
using Stripe;
using Syncfusion.Licensing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringDb"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.AccessDeniedPath = "/Account/AccessDenied";
    option.LoginPath = "/Account/Login";
});
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequiredLength = 6;
    
});
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["LicenceKey:SyncfusionKey"]);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
