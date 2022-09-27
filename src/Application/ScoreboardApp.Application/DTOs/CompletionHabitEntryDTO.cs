using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.DTOs
{
    public sealed class CompletionHabitEntryDTO : IMapFrom<CompletionHabitEntry>
    {
        public string? Description { get; set; }
        public IList<CompletionHabitEntryDTO> HabitEntries { get; set; }
        public Guid HabitTrackerId { get; set; }
        public string? Title { get; set; }
    }
}