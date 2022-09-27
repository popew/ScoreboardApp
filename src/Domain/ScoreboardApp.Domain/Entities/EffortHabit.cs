using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortHabit : BaseEntity, IHabit<EffortHabitEntry>
    {
        public string? Unit { get; set; }

        public double? AverageGoal { get; set; }

        public EffortHabitSubtype Subtype { get; set; } = EffortHabitSubtype.None;
        public string? Description { get; set; }
        public IList<EffortHabitEntry> HabitEntries { get; set; } = new List<EffortHabitEntry>();
        public HabitTracker HabitTracker { get; set; }
        public Guid HabitTrackerId { get; set; }
        public string? Title { get; set; }
    }
}