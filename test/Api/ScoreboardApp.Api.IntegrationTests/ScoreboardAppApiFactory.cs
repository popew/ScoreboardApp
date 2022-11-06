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

namespace ScoreboardApp.Api.IntegrationTests
{
    public class ScoreboardAppApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        private readonly TestcontainerDatabase _apiDbContainer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "ApiDb",
                Password = "Pa@@word123"
            })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPortBinding(1433, 1433)
            .Build();

        private readonly TestcontainerDatabase _identityDbContainer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "IdentityDb",
                Password = "Pa@@word123"
            })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPortBinding(1444, 1433)
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

        public async Task InitializeAsync()
        {
            await _apiDbContainer.StartAsync();
            await _identityDbContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _apiDbContainer.DisposeAsync();
            await _identityDbContainer.DisposeAsync();
        }
    }
}
