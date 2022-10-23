using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScoreboardApp.Infrastructure.Persistence;

namespace ScoreboardApp.Infrastructure.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Create Identity DB if not exist
        /// </summary>
        /// <param name="builder"></param>
        public static void ExecuteApplicationDbContextMigrations(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dbContext = services.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();
            }
        }
    }
}
