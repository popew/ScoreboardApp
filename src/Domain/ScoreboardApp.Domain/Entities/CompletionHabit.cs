using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Entities.Interfaces;

namespace ScoreboardApp.Domain.Entities
{
    public class CompletionHabit : BaseAuditableEntity, IHabit<CompletionHabitEntry>
    {
        public string? Description { get; set; }
        public IList<CompletionHabitEntry> HabitEntries { get; set; } = new List<CompletionHabitEntry>();
        public HabitTracker HabitTracker { get; set; } = default!;
        public Guid HabitTrackerId { get; set; }
        public required string Title { get; set; }
        public required string UserId { get; set; }
    }
}