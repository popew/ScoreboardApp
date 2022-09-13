using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ScoreboardApp.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}