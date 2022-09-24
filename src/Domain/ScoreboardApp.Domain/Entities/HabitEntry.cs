using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class HabitEntry<THabit> : BaseEntity
    {
        public Guid HabitId { get; set; }
        public THabit Habit { get; set; }

        public DateOnly EntryDate { get; set; } 
    }
}