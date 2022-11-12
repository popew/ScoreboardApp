using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ScoreboardApp.Infrastructure.CustomIdentityService;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.Persistence;
using ScoreboardApp.Infrastructure.Telemetry.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace ScoreboardApp.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddCustomIdentityService(configuration);

            // Configure HealthChecks
            services.AddHealthChecks()
                    .AddSqlServer(name: "ApiDatabase",
                                  connectionString: configuration.GetConnectionString("DefaultConnection"),
                                  failureStatus: HealthStatus.Degraded)
                    .AddSqlServer(name: "IdentityDatabase",
                                  connectionString: configuration.GetConnectionString("CustomIdentityDatabase"),
                                  failureStatus: HealthStatus.Degraded);

            // Configure Telemetry
            services.Configure<TelemetryOptions>(configuration.GetSection(nameof(TelemetryOptions)));

            var telemetryOptions = configuration.GetSection(nameof(TelemetryOptions)).Get<TelemetryOptions>();

            if (telemetryOptions.IsEnabled == true)
            {
                services.AddOpenTelemetryTracing(builder =>
                {
                    builder
                        .SetResourceBuilder(ResourceBuilder
                        .CreateDefault()
                        .AddService("ScoreboardApp"))
                        .AddZipkinExporter(options =>
                        {
                            options.Endpoint = new Uri(telemetryOptions.Endpoint);
                        })
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddSqlClientInstrumentation();
                });
            }

            return services;
        }
    }
}