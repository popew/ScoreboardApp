using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity
{
    public class IdentityDbContextDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDbContextDataSeeder> _logger;

        public IdentityDbContextDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDbContextDataSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Seed roles in the Identity database.
        /// </summary>
        /// <param name="roleNames">List of roles to add to the identity database if they don't exists</param>
        /// <returns></returns>
        public async Task SeedRolesAsync(IEnumerable<string> roleNames)
        {
            foreach (string roleName in roleNames)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(roleName);

                if(!roleExists)
                {
                    _logger.LogInformation("Creating IdentityRole: {roleName}", roleName);

                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        /// <summary>
        ///     Seed user in the Identity database. Call SeedRolesAsync beforehand. If userName is not specified, the userName is the same as email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="roleNames"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task SeedUserAsync(string email, string password, IEnumerable<string> roleNames, string? userName = null)
        {
            var user = new ApplicationUser()
            {
                UserName = userName ?? email,
                Email = email,
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(user, password);
            
            var identityUser = await _userManager.FindByNameAsync(user.UserName);

            foreach(string roleName in roleNames)
            {
                _logger.LogInformation("Adding {userName} to role {roleName}", identityUser.UserName, roleName);

                await _userManager.AddToRoleAsync(identityUser, roleName);
            }
        }
    }
}