using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Infrastructure.Persistence
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<HabitTracker> HabitTrackers;
        public DbSet<Habit> Habits;
        public DbSet<HabitEntry> HabitEntries;
    }
}