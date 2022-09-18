using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    public class HabitTracker : BaseEntity
    {
        public string Name { get; set; } = default!;
        public int Order { get; set; }

        public IList<Habit> Habits = new List<Habit>();
    }
}
