using ScoreboardApp.Domain.Entities.Interfaces;

namespace ScoreboardApp.Domain.Entities
{
    public class CompletionHabitEntry : BaseEntity, IHabitEntry<CompletionHabit>
    {
        public bool Completion { get; set; }
        public DateOnly EntryDate { get; set; }
        public CompletionHabit Habit { get; set; }
        public Guid HabitId { get; set; }
    }
}