using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Create Identity DB if not exist
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureIdentityDbIsCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

                dbContext.Database.EnsureCreated();
            }
        }

        /// <summary>
        ///     Seed Identity data
        /// </summary>
        /// <param name="builder"></param>
        public static async Task SeedIdentityDataAsync(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await ApplicationIdentityDbContextDataSeed.SeedAsync(userManager, roleManager);
            }
        }
    }
}