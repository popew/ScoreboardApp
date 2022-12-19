using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Entities.Interfaces;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortHabit : BaseAuditableEntity, IHabit<EffortHabitEntry>
    {
        public string? Unit { get; set; }

        public double? AverageGoal { get; set; }

        public EffortHabitSubtype Subtype { get; set; } = EffortHabitSubtype.None;
        public string? Description { get; set; }
        public IList<EffortHabitEntry> HabitEntries { get; set; } = new List<EffortHabitEntry>();
        public HabitTracker HabitTracker { get; set; } = default!;
        public Guid HabitTrackerId { get; set; }
        public required string Title { get; set; }
        public required string UserId { get; set; }
    }
}