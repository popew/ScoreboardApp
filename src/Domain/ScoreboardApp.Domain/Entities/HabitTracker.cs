using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class HabitTracker : BaseEntity
    {
        public string Title { get; set; } = default!;
        public Priority Priority { get; set; } = Priority.None;

        public IList<Habit> Habits { get; private set; } = new List<Habit>();
    }
}