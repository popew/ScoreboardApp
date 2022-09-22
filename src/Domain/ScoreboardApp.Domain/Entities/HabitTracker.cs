using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    public class HabitTracker : BaseEntity
    {
        public string Title { get; set; } = default!;
        public uint? Order { get; set; }

        public IList<Habit> Habits { get; private set; } = new List<Habit>();
    }
}