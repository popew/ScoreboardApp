using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Infrastructure.Persistence
{
    public  interface IApplicationDbContext
    {
        DbSet<CompletionHabitEntry> CompletionHabitEntries { get; set; }
        DbSet<CompletionHabit> CompletionHabits { get; set; }
        DbSet<EffortHabitEntry> EffortHabitEntries { get; set; }
        DbSet<EffortHabit> EffortHabits { get; set; }
        DbSet<HabitTracker> HabitTrackers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}