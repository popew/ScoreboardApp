using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Commons.Interfaces;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class HabitTracker : BaseAuditableEntity, IOwnedEntity
    {
        public string? Title { get; set; }
        public Priority Priority { get; set; } = Priority.None;

        public IList<CompletionHabit> CompletionHabits { get; private set; } = new List<CompletionHabit>();
        public IList<EffortHabit> EffortHabits { get; private set; } = new List<EffortHabit>();

        public required string UserId { get; set; }
    }
}