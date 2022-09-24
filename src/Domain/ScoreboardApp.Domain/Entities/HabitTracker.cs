using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class HabitTracker : BaseEntity
    {
        public string Title { get; set; } = default!;
        public Priority Priority { get; set; } = Priority.None;

        public IList<CompletionHabit> CompletionHabits { get; private set; } = new List<CompletionHabit>();
        public IList<EffortHabit> EffortHabits { get; private set; } = new List<EffortHabit>();
    }
}