using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Application.HabitTrackers.DTOs
{
    public sealed class EffortHabitEntryDTO : IMapFrom<EffortHabitEntry>
    {
        public Guid Id { get; set; }
        public double Effort { get; set; }

        public double? SessionGoal { get; set; }
        public DateOnly EntryDate { get; set; }
        public Guid HabitId { get; set; }
    }
}