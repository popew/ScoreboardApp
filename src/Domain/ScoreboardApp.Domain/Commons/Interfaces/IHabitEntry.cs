using ScoreboardApp.Domain.Commons.Interfaces;

namespace ScoreboardApp.Domain.Entities.Interfaces
{
    public interface IHabitEntry<THabit> : IOwnedEntity
    {
        DateOnly EntryDate { get; set; }
        THabit Habit { get; set; }
        Guid HabitId { get; set; }
    }
}