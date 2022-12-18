using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.Commons.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<CompletionHabitEntry> CompletionHabitEntries { get; }
        DbSet<CompletionHabit> CompletionHabits { get; }
        DbSet<EffortHabitEntry> EffortHabitEntries { get; }
        DbSet<EffortHabit> EffortHabits { get; }
        DbSet<HabitTracker> HabitTrackers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}