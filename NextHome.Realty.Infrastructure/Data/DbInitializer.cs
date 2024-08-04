using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Domain.Entities;

namespace NextHome.Realty.Persistence.Data;

public class DbInitializer(
    ApplicationDbContext db,
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager) : IDbInitializer
{
    public async Task Initialize()
    {
        try
        {
            if ((await db.Database.GetPendingMigrationsAsync()).Any()) await db.Database.MigrateAsync();
            if (!await roleManager.RoleExistsAsync(SD.RoleType.Admin.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.RoleType.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(SD.RoleType.Customer.ToString()));

                userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "dotnet@gmail.com",
                    Email = "dotnet@gmail.com",
                    Name = "Abdulwaisa",
                    NormalizedUserName = "ABDULWAISA",
                    NormalizedEmail = "DOTNET@GMAIL.COM",
                    PhoneNumber = "771800613"
                }, "Abc12345%");

                var user = await db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == "dotnet@gmail.com");
                await userManager.AddToRolesAsync(user!,
                    new[] { SD.RoleType.Admin.ToString(), SD.RoleType.Customer.ToString() });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}