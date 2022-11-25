using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.DTOs
{
    public sealed class EffortHabitDTO : IMapFrom<EffortHabit>
    {
        public Guid Id { get; set; }
        public string? Unit { get; set; }

        public double? AverageGoal { get; set; }

        public EffortHabitSubtypeMapping Subtype { get; set; }
        public string? Description { get; set; }
        public IList<EffortHabitEntryDTO> HabitEntries { get; set; } = new List<EffortHabitEntryDTO>();
        public Guid HabitTrackerId { get; set; }
        public string? Title { get; set; }
    }
}