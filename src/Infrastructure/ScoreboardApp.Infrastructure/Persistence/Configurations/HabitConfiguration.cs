using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public abstract class HabitConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Habit
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(h => h.HabitTracker)
                .IsRequired();

            builder.Property(h => h.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(h => h.Description)
                .HasMaxLength(400);
        }
    }

    public class CompletionHabitConfiguration : HabitConfiguration<CompletionHabit>
    {
        public override void Configure(EntityTypeBuilder<CompletionHabit> builder)
        {
            base.Configure(builder);
        }
    }

    public class EffortHabitConfiguration : HabitConfiguration<EffortHabit>
    {
        public override void Configure(EntityTypeBuilder<EffortHabit> builder)
        {
            base.Configure(builder);

            builder.Property(h => h.HabitSubtype)
                .IsRequired();

            builder.Property(h => h.Unit)
                .HasMaxLength(20);
        }
    }
}
