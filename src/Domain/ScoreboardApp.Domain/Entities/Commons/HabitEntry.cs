namespace ScoreboardApp.Domain.Entities.Commons
{
    public abstract class HabitEntry : BaseEntity
    {
        public DateOnly EntryDate { get; set; }
    }
}