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
    internal class CompletionHabit : Habit
    {
        [ForeignKey("Id")]
        public IList<CompletionHabitEntry> HabitEntries { get; set; } = new List<CompletionHabitEntry>();
    }
}
