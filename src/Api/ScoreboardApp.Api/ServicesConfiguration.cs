using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using ScoreboardApp.Api.Filters;
using ScoreboardApp.Api.Services;
using ScoreboardApp.Application.Commons.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScoreboardApp.Api
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDateOnlyTimeOnlyStringConverters();

            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ApiVersionReader = new HeaderApiVersionReader("X-api-version");
                options.ReportApiVersions = true;
            });

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}