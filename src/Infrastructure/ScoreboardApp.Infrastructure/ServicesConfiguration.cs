using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ScoreboardApp.Infrastructure.Persistence;
using System.ComponentModel.DataAnnotations;

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

            services.AddHealthChecks()
                    .AddSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection"),
                                  failureStatus: HealthStatus.Degraded);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Audience = configuration["AAD:ResourceId"]; // Audience -> the API
                        options.Authority = $"{configuration["AAD:InstanceId"]}{configuration["AAD:TenantId"]}";
                    });


            return services;
        }
    }
}