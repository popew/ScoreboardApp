using ScoreboardApp.Api.Filters;

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
