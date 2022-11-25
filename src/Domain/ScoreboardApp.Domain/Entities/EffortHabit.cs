using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Commons.Interfaces;
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
        public HabitTracker HabitTracker { get; set; }
        public Guid HabitTrackerId { get; set; }
        public string? Title { get; set; }
        public string UserId { get; set; }
    }
}                       