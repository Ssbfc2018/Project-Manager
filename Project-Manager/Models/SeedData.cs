using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;

namespace Project_Manager.Models
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!context.Roles.Any())
            {
                List<string> seededRoles = new List<string>()
                {
                    "Admin",
                    "Manager",
                    "Developer"
                };
                
                foreach(string s in seededRoles)
                {
                    await roleManager.CreateAsync(new IdentityRole(s));
                }
            }

            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();

                ApplicationUser firstAdmin = new ApplicationUser
                {
                    Email = "admin@123.ca",
                    NormalizedEmail = "ADMIN@123.CA",
                    UserName = "admin@123.ca",
                    NormalizedUserName = "ADMIN@123.CA",
                    EmailConfirmed = true
                };

                var hashedPassword = passwordHasher.HashPassword(firstAdmin, "`1Qazxsw");
                firstAdmin.PasswordHash = hashedPassword;

                await userManager.CreateAsync(firstAdmin);
                await userManager.AddToRoleAsync(firstAdmin, "Admin");
            }

            context.SaveChanges();
        }
    }
}
