namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class HabitEntry: BaseEntity
    {
        public Guid HabitId { get; set; }
        public DateOnly EntryDate { get; set; } 
    }
}