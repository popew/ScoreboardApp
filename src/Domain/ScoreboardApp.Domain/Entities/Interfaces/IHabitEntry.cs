namespace ScoreboardApp.Domain.Entities.Interfaces
{
    public interface IHabitEntry<THabit>
    {
        DateOnly EntryDate { get; set; }
        THabit Habit { get; set; }
        Guid HabitId { get; set; }
    }
}