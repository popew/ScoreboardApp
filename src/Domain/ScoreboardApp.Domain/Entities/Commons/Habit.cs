using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class Habit : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public HabitType HabitType { get; set; }
        public double? Goal { get; set; }
        public HabitTracker HabitTracker { get; set; }

    }
}
