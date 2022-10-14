using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Interfaces;
using ScoreboardApp.Domain.Enums;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public abstract class HabitConfiguration<THabit, TEntry> : BaseEntityConfiguration<THabit>, IEntityTypeConfiguration<THabit>
        where THabit : BaseEntity, IHabit<TEntry> 
        where TEntry : BaseEntity, IHabitEntry<THabit>
    {
        public override void Configure(EntityTypeBuilder<THabit> builder)
        {
            base.Configure(builder);

            builder.Property(h => h.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(h => h.Description)
                .HasMaxLength(400);

            builder.Property(h => h.HabitTrackerId)
                .IsRequired();

            builder.HasMany(h => h.HabitEntries)
                .WithOne(e => e.Habit)
                .HasForeignKey(e => e.HabitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class CompletionHabitConfiguration : HabitConfiguration<CompletionHabit, CompletionHabitEntry>
    {
        public override void Configure(EntityTypeBuilder<CompletionHabit> builder)
        {
            base.Configure(builder);
        }
    }

    public class EffortHabitConfiguration : HabitConfiguration<EffortHabit, EffortHabitEntry>
    {
        public override void Configure(EntityTypeBuilder<EffortHabit> builder)
        {
            base.Configure(builder);

            builder.Property(h => h.Subtype)
                .IsRequired();

            builder.Property(h => h.Unit)
                .HasMaxLength(20);
        }
    }
}