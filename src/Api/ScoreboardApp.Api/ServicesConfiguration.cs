using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using ScoreboardApp.Api.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            .AddJsonOptions(options =>
            {
                options.UseDateOnlyTimeOnlyStringConverters(); // Correctly serialize and deserialize DateOnly type

                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Use enums as strings instead of numbers in API
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ApiVersionReader = new HeaderApiVersionReader("X-api-version");
                options.ReportApiVersions = true;
            });

            return services;
        }
    }
}