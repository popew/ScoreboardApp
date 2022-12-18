using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.DTOs
{
    public sealed class CompletionHabitDTO : IMapFrom<CompletionHabit>
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public IList<CompletionHabitEntryDTO> HabitEntries { get; set; } = new List<CompletionHabitEntryDTO>();
        public Guid HabitTrackerId { get; set; }
        public string? Title { get; set; }
    }
}