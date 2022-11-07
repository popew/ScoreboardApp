using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoreboardApp.Api;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ScoreboardApp.Infrastructure.Persistence;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using System.Net.Http.Json;
using ScoreboardApp.Application.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;

namespace ScoreboardApp.Api.IntegrationTests
{
    public class ScoreboardAppApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        public const string DefaultTestPassword = "Pa@@word123";

        public readonly TestUser AdminTestUser = new("test_admin@scoreboardapp.com", DefaultTestPassword, new string[] { Roles.Administrator, Roles.User });
        public readonly TestUser NormalTestUser = new("test_testuser@scoreboardapp.com", DefaultTestPassword, new string[] { Roles.User });

        private readonly TestcontainerDatabase _apiDbContainer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "ApiDb",
                Password = DefaultTestPassword
            })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        private readonly TestcontainerDatabase _identityDbContainer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "IdentityDb",
                Password = DefaultTestPassword
            })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.ConfigureTestServices(services =>
            {
                // Remove all DbContextOptions and DbContext services and replace them with test containers
                foreach (var option in services.Where(s => s.ServiceType.BaseType == typeof(DbContextOptions)).ToList())
                {
                    services.Remove(option);
                }

                services.Remove<ApplicationDbContext>()
                        .AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseSqlServer($"{_apiDbContainer.ConnectionString};TrustServerCertificate=true");
                        });

                services.Remove<ApplicationIdentityDbContext>()
                        .AddDbContext<ApplicationIdentityDbContext>(options =>
                        {
                            options.UseSqlServer($"{_identityDbContainer.ConnectionString};TrustServerCertificate=true");
                        });
            });
        }

        private async Task SeedTestUsersAsync()
        {
            using var scope = Services.CreateScope();
            {
                var services = scope.ServiceProvider;

                var dataSeeder = services.GetRequiredService<IdentityDbContextDataSeeder>();

                await dataSeeder.SeedRolesAsync(Roles.RolesSupported);

                await dataSeeder.SeedUserAsync(AdminTestUser.Email, AdminTestUser.Password, AdminTestUser.Roles);
                await dataSeeder.SeedUserAsync(NormalTestUser.Email, NormalTestUser.Password, NormalTestUser.Roles);

                await Task.WhenAll(GetTokenForTestUser(AdminTestUser), GetTokenForTestUser(NormalTestUser));
            }
        }

        private async Task GetTokenForTestUser(TestUser testUser)
        {
            using var scope = Services.CreateScope();
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var tokenGenerator = services.GetRequiredService<ITokenService>();

                var applicationUser = await userManager.FindByNameAsync(testUser.UserName);
                var roles = await userManager.GetRolesAsync(applicationUser);

                testUser.Token = await tokenGenerator.GenerateJwtTokenAsync(applicationUser, roles);
            }

            // Generating token using API:
            //using(var client = CreateClient())
            //{
            //    var httpResponse = await client.PostAsJsonAsync("api/Users/authenticate", new AuthenticateCommand() { UserName = testUser.UserName, Password = testUser.Password });

            //    var responseObject = await httpResponse.Content.ReadFromJsonAsync<AuthenticateCommandResponse>();

            //    testUser.Token = responseObject!.Token;
            //}
        }

        public async Task InitializeAsync()
        {
            await Task.WhenAll(_apiDbContainer.StartAsync(), _identityDbContainer.StartAsync());

            await SeedTestUsersAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _apiDbContainer.DisposeAsync();
            await _identityDbContainer.DisposeAsync();
        }
    }
}
