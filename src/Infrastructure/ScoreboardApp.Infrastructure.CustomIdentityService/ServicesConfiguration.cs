using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;
using ScoreboardApp.Infrastructure.Identity.Services;
using System.Security.Claims;
using System.Text;

namespace ScoreboardApp.Infrastructure.CustomIdentityService
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddCustomIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<TokenSettings>()
                    .Bind(configuration.GetSection(nameof(TokenSettings)))
                    .Validate(options =>
                    {
                        return !string.IsNullOrEmpty(options.Secret);
                    })
                    .ValidateOnStart();

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CustomIdentityDatabase")!,
                                     providerOptions => providerOptions.EnableRetryOnFailure());
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddDefaultTokenProviders()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddSignInManager<SignInManager<ApplicationUser>>()
                    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            services.Configure<IdentityOptions>(
                options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                    // Identity : Default password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IdentityDbContextDataSeeder>();

            // Authentication
            Action<JwtBearerOptions> jwtBearerOptions = options =>
            {
                var tokenSettings = configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();
                byte[] secret = Encoding.ASCII.GetBytes(tokenSettings!.Secret);

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.ClaimsIssuer = tokenSettings.Issuer;
                options.IncludeErrorDetails = true;
                options.Validate(JwtBearerDefaults.AuthenticationScheme);
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenSettings.Issuer,
                        ValidAudience = tokenSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true
                    };
            };

            services.Configure<JwtBearerOptions>(jwtBearerOptions);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions);

            return services;
        }
    }
}