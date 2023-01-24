using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Create Identity DB if not exist
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureIdentityDbMigrations(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
                var cs = dbContext.Database.GetDbConnection().ConnectionString;
                dbContext.Database.Migrate();
            }
        }

        /// <summary>
        ///     Seed Identity data
        /// </summary>
        /// <param name="builder"></param>
        public static async Task SeedIdentityDataAsync(this IApplicationBuilder builder, IConfiguration configuration)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dataSeeder = services.GetRequiredService<IdentityDbContextDataSeeder>();

                await dataSeeder.SeedRolesAsync(Roles.RolesSupported);
                await dataSeeder.SeedUserAsync("dev_admin@scoreboardapp.com", configuration.GetValue<string>("Seeding:AdminPassword")!, new string[] { Roles.Administrator, Roles.User });
                await dataSeeder.SeedUserAsync("dev_testuser@scoreboardapp.com", configuration.GetValue<string>("Seeding:TestUserPassword")!, new string[] { Roles.User });
            }
        }
    }
}