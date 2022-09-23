using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    public class CompletionHabitEntry : HabitEntry
    {
        public CompletionHabit Habit { get; set; }
        public bool Completion { get; set; }
    }
}