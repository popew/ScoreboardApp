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
    internal class Habit : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsChecked { get; set; }

        public int GoalInPercent { get;}

        [ForeignKey("Id")]
        public HabitTrackerEntry HabitTrackerEntry { get; set; }
    }
}
