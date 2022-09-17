using ScoreboardApp.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Entities
{
    public class HabitTracker : BaseEntity
    {
        public int Order { get; set; }

        public IList<Habit> HabitTrackerEntries = new List<Habit>();
    }
}
