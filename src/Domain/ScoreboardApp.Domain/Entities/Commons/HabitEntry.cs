using ScoreboardApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class HabitEntry : BaseEntity
    {
        [Required]
        [ForeignKey("Id")]
        public Habit Habit { get; set; }

        [Required]
        public DateOnly EntryDate { get; set; }

        [Required]
        public EntryType EntryType { get; set; }
    }
}