using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortHabit : Habit<EffortHabitEntry>
    {
        public string? Unit { get; set; }

        public double? AverageGoal { get; set; }

        public EffortHabitSubtype Subtype { get; set; } = EffortHabitSubtype.None;

    }
}