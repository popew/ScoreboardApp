using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Infrastructure.Persistence.Converters;
using ScoreboardApp.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace ScoreboardApp.Infrastructure.Persistence
{
    public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<HabitTracker> HabitTrackers => Set<HabitTracker>();
        public DbSet<CompletionHabit> CompletionHabits => Set<CompletionHabit>();
        public DbSet<EffortHabit> EffortHabits => Set<EffortHabit>();
        public DbSet<CompletionHabitEntry> CompletionHabitEntries => Set<CompletionHabitEntry>();

        public DbSet<EffortHabitEntry> EffortHabitEntries => Set<EffortHabitEntry>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

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