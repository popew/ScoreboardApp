using ScoreboardApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class Habit : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required]
        public HabitType HabitType { get; set; }
        public double? Goal { get; set; }

        [ForeignKey("Id")]
        public HabitTracker HabitTracker { get; set; }

    }
}
