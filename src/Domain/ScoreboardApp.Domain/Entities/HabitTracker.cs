using ScoreboardApp.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Entities
{
    internal class HabitTracker : BaseEntity
    {
        IList<HabitTrackerEntry> HabitTrackerEntries = new List<HabitTrackerEntry>();
    }
}
