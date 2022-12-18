using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.DTOs
{
    public sealed class CompletionHabitEntryDTO : IMapFrom<CompletionHabitEntry>
    {
        public Guid Id { get; init; }
        public bool Completion { get; init; }
        public DateOnly EntryDate { get; init; }
        public CompletionHabitDTO Habit { get; init; } = default!;
        public Guid HabitId { get; init; }
    }
}