using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Infrastructure.CustomIdentityService;
using ScoreboardApp.Infrastructure.Persistence;
using ScoreboardApp.Infrastructure.Services;
using ScoreboardApp.Infrastructure.Telemetry.Options;

namespace ScoreboardApp.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                     providerOptions => providerOptions.EnableRetryOnFailure());
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

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

            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}