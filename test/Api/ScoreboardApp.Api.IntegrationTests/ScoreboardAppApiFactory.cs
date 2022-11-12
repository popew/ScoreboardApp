using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;
using ScoreboardApp.Infrastructure.Persistence;

namespace ScoreboardApp.Api.IntegrationTests
{
    public class ScoreboardAppApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        public const string DefaultTestPassword = "Pa@@word123";

        public readonly TestUser AdminTestUser = new("test_admin@scoreboardapp.com", DefaultTestPassword, new string[] { Roles.Administrator, Roles.User });
        public readonly TestUser NormalTestUser = new("test_testuser@scoreboardapp.com", DefaultTestPassword, new string[] { Roles.User });

        public readonly Faker<TestUser> TestUserGenerator = new Faker<TestUser>()
            .RuleFor(x => x.Email, faker => faker.Internet.Email())
            .RuleFor(x => x.Password, faker => DefaultTestPassword)
            .RuleFor(x => x.Roles, faker => new string[] { Roles.User })
            .RuleFor(x => x.UserName, faker=> faker.Internet.UserName());

        private readonly TestcontainerDatabase _testDbServer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "ApiDb",
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
                            options.UseSqlServer($"{_testDbServer.ConnectionString};TrustServerCertificate=true");
                        });

                services.Remove<ApplicationIdentityDbContext>()
                        .AddDbContext<ApplicationIdentityDbContext>(options =>
                        {
                            options.UseSqlServer($"{_testDbServer.ConnectionString};TrustServerCertificate=true");
                        });
            });
        }

        private async Task SeedTestUsersAsync()
        {
            await Task.WhenAll(SeedTestUserAsync(AdminTestUser), SeedTestUserAsync(NormalTestUser));
            await Task.WhenAll(GetTokenForTestUser(AdminTestUser), GetTokenForTestUser(NormalTestUser));
        }

        public async Task SeedTestUserAsync(TestUser testUser)
        {
            using var scope = Services.CreateScope();
            {
                var services = scope.ServiceProvider;

                var dataSeeder = services.GetRequiredService<IdentityDbContextDataSeeder>();

                await dataSeeder.SeedRolesAsync(Roles.RolesSupported);

                await dataSeeder.SeedUserAsync(testUser.Email, testUser.Password, testUser.Roles, testUser.UserName);
            }
        }

        public async Task GetTokenForTestUser(TestUser testUser)
        {
            using var scope = Services.CreateScope();
            {
                var services = scope.ServiceProvider;

                var userService = services.GetRequiredService<IUserService>();

                var applicationUser = await userService.GetUserByUserNameAsync(testUser.UserName);
                var tokenResponse = await userService.GenerateTokensForUserAsync(applicationUser!);

                testUser.Token = tokenResponse.Token;
                testUser.RefreshToken = tokenResponse.RefreshToken;
                testUser.RefreshTokenExpiry = tokenResponse.RefreshTokenExpiry;
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
            await _testDbServer.StartAsync();

            await SeedTestUsersAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _testDbServer.DisposeAsync();
        }
    }
}