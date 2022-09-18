using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class CompletionHabit : Habit
    {
        public CompletionHabit()
        {
            HabitType = HabitType.Completion;
        }
        public IList<CompletionHabitEntry> HabitEntries { get; set; } = new List<CompletionHabitEntry>();
    }
}
