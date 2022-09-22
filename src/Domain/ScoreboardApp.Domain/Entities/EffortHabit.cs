using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortHabit : Habit
    {
        public string? Unit { get; set; }

        public double? AverageGoal { get; set; }

        public HabitSubtype HabitSubtype { get; set; } = HabitSubtype.None;

        public IList<EffortHabitEntry> HabitEntries { get; set; } = new List<EffortHabitEntry>();
    }
}