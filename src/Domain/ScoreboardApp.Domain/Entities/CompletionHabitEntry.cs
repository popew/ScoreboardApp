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
    internal class CompletionHabitEntry : HabitEntry
    {
        [Required]
        public bool Completion { get; set; }

        public IList<EffortHabitEntry> HabitEntries { get; set; } = new List<EffortHabitEntry>();
    }
}
