using Microsoft.Extensions.DependencyInjection;
using ScoreboardApp.Api.Filters;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ScoreboardApp.Api
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
                options.UseDateOnlyTimeOnlyStringConverters();
            })
            .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());



            return services;
        }
    }
}
