using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Persistence
{
    public sealed class ApplicationIdentityDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationIdentityDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}