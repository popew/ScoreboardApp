using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public abstract class HabitEntryConfiguration<T> : IEntityTypeConfiguration<T>
        where T : HabitEntry
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.EntryDate)
                .IsRequired();
        }
    }

    public class EffortHabitEntryConfiguration : HabitEntryConfiguration<EffortHabitEntry>
    {
        public override void Configure(EntityTypeBuilder<EffortHabitEntry> builder)
        {
            base.Configure(builder);

            //builder.Property(e => e.Habit)
            //    .IsRequired();

            builder.Property(e => e.Effort)
                .IsRequired();
        }
    }

    public class CompletionHabitEntryConfiguration : HabitEntryConfiguration<CompletionHabitEntry>
    {
        public override void Configure(EntityTypeBuilder<CompletionHabitEntry> builder)
        {
            base.Configure(builder);

            //builder.Property(e => e.Habit)
            //       .IsRequired();
        }
    }
}