using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Entities.Interfaces;

namespace ScoreboardApp.Domain.Entities
{
    public class CompletionHabit : BaseAuditableEntity, IHabit<CompletionHabitEntry>
    {
        public string? Description { get; set; }
        public IList<CompletionHabitEntry> HabitEntries { get; set; }
        public HabitTracker HabitTracker { get; set; }
        public Guid HabitTrackerId { get; set; }
        public string? Title { get; set; }
    }
}