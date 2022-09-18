using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortBasedHabit : Habit
    {
        public string? Unit { get; set; }

        public double? AverageGoal { get; set; }

        public IList<EffortHabitEntry> HabitEntries { get; set; } = new List<EffortHabitEntry>();
    }
}
