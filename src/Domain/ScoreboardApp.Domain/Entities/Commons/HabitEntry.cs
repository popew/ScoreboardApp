namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class HabitEntry : BaseEntity
    {
        public Habit Habit { get; set; }

        public DateOnly EntryDate { get; set; }
    }
}