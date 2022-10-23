using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using ScoreboardApp.Infrastructure.CustomIdentityService;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.Persistence;
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

            services.AddHealthChecks()
                    .AddSqlServer(connectionString: configuration.GetConnectionString("DefaultConnection"),
                                  failureStatus: HealthStatus.Degraded);

            services.AddCustomIdentityService(configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var tokenSettings = configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();
                byte[] secret = Encoding.ASCII.GetBytes(tokenSettings.Secret);

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.ClaimsIssuer = tokenSettings.Issuer;
                options.IncludeErrorDetails = true;
                options.Validate(JwtBearerDefaults.AuthenticationScheme);
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenSettings.Issuer,
                        ValidAudience = tokenSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true
                    };
            });


            return services;
        }
    }
}