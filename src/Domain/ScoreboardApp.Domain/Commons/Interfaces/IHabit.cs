using ScoreboardApp.Domain.Commons.Interfaces;

namespace ScoreboardApp.Domain.Entities.Interfaces
{
    public interface IHabit<TEntry> : IOwnedEntity
    {
        string? Description { get; set; }
        IList<TEntry> HabitEntries { get; set; }
        HabitTracker HabitTracker { get; set; }
        Guid HabitTrackerId { get; set; }
        string Title { get; set; }
    }
}