using ScoreboardApp.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortBasedHabit : Habit
    {
        public string? Unit { get; set; }

        [ForeignKey("Id")]
        public IList<EffortHabitEntry> HabitEntries { get; set; } = new List<EffortHabitEntry>();
    }
}
