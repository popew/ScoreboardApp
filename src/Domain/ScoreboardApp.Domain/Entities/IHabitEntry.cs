namespace ScoreboardApp.Domain.Entities.Commons
{
    public interface IHabitEntry<THabit>
    {
        DateOnly EntryDate { get; set; }
        THabit Habit { get; set; }
        Guid HabitId { get; set; }
    }
}