using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    internal class CompletionHabitEntry : HabitEntry
    {
        public bool Completion { get; set; }

        public IList<EffortHabitEntry> HabitEntries { get; set; } = new List<EffortHabitEntry>();
    }
}
