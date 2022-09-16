using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Entities
{
    internal class HabitTrackerEntry
    {
        [ForeignKey("Id")]
        public HabitTracker HabitTracker { get; set; }
        public DateTime Date { get; set; }

        public string? Note { get; set; }
        public IList<Habit> Habits { get; set; } = new List<Habit>();
    }
}
