using ScoreboardApp.Application.Commons.Enums;
using ScoreboardApp.Application.Commons.Mappings;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.DTOs
{
    public sealed class HabitTrackerDTO : IMapFrom<HabitTracker>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public PriorityMapping Priority { get; set; }

        public IList<CompletionHabitDTO> CompletionHabits { get; private set; } = new List<CompletionHabitDTO>();
        public IList<EffortHabitDTO> EffortHabits { get; private set; } = new List<EffortHabitDTO>();
    }
}
