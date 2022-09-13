using Microsoft.EntityFrameworkCore;

namespace ScoreboardApp.Infrastructure.Persistence
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}