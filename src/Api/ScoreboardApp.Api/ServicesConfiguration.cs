using Microsoft.Extensions.DependencyInjection;
using ScoreboardApp.Api.Filters;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Extensions.Hosting;
using OpenTelemetry.Resources;

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

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ApiVersionReader = new HeaderApiVersionReader("X-api-version");
                options.ReportApiVersions = true;
            });

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder
                    .CreateDefault()
                    .AddService("ScoreboardApp"))
                    .AddZipkinExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:9411/api/v2/spans"); // Hardcoding it for now, because the library throws for some reason when the value is read from appsettings
                    })
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddSqlClientInstrumentation();
            });

            return services;
        }
    }
}
