using ScoreboardApp.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Entities
{
    public class EffortHabitEntry : HabitEntry
    {
        [Required]
        public double Effort { get; set; }

        public double? SessionGoal { get; set; }
    }
}
