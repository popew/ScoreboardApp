using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Commons.Interfaces;
using ScoreboardApp.Domain.Entities.Interfaces;

namespace ScoreboardApp.Domain.Entities
{
    public class CompletionHabitEntry : BaseAuditableEntity, IHabitEntry<CompletionHabit>
    {
        public bool Completion { get; set; }
        public DateOnly EntryDate { get; set; }
        public CompletionHabit Habit { get; set; } = default!;
        public Guid HabitId { get; set; }
        public required string UserId { get; set; }
    }
}