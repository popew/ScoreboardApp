using Microsoft.AspNetCore.Identity;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity
{
    public static class ApplicationIdentityDbContextDataSeed
    {
        /// <summary>
        ///     Seed users and roles in the Identity database.
        /// </summary>
        /// <param name="userManager">ASP.NET Core Identity User Manager</param>
        /// <param name="roleManager">ASP.NET Core Identity Role Manager</param>
        /// <returns></returns>
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Add roles supported
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
            await roleManager.CreateAsync(new IdentityRole(Roles.User));

            // New admin user
            string adminUserName = "admin@test.com";

            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminUserName,
                EmailConfirmed = true
            };

            // Add new user and their role
            await userManager.CreateAsync(adminUser, "DefaultP@ssword123"); // TODO: Better password choosing?
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, Roles.Administrator);
        }
    }
}