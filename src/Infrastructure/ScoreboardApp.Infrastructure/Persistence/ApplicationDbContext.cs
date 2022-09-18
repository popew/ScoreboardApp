using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Relational;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Infrastructure.Persistence.Converters;
using System.Reflection;

namespace ScoreboardApp.Infrastructure.Persistence
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<HabitTracker> HabitTrackers { get; set; }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<CompletionHabit> CompletionHabits { get; set; }
        public DbSet<EffortHabit> EffortHabits { get; set; }

        public DbSet<HabitEntry> HabitEntries { get; set; }
        public DbSet<CompletionHabitEntry> CompletionHabitEntries { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                   .HaveConversion<DateOnlyConverter>()
                   .HaveColumnType("date");
        }
    }
}