using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public abstract class Habit<TEntry> : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        public Guid HabitTrackerId { get; set; }
        public HabitTracker HabitTracker { get; set; }

        public IList<TEntry> HabitEntries { get; set; } = new List<TEntry>();
    }

}