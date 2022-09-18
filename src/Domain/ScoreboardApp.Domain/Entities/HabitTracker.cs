using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    public class HabitTracker : BaseEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }

        public IList<Habit> HabitTrackerEntries = new List<Habit>();
    }
}
