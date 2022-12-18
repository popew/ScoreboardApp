using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Commons.Interfaces;
using ScoreboardApp.Domain.Entities.Interfaces;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortHabitEntry : BaseAuditableEntity, IHabitEntry<EffortHabit>
    {
        public double Effort { get; set; }

        public double? SessionGoal { get; set; }
        public DateOnly EntryDate { get; set; }
        public EffortHabit Habit { get; set; } = default!;
        public Guid HabitId { get; set; }
        public required string UserId { get; set; }
    }
}