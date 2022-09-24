using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    public class CompletionHabitEntry : HabitEntry<CompletionHabit>
    {
        public bool Completion { get; set; }
    }
}