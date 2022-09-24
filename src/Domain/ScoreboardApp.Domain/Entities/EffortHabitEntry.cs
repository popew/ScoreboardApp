using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortHabitEntry : HabitEntry<EffortHabit>
    {
        public double Effort { get; set; }

        public double? SessionGoal { get; set; }
    }
}