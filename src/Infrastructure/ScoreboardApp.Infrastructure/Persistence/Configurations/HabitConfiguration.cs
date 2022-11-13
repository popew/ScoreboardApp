using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Interfaces;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public abstract class HabitConfiguration<THabit, TEntry> : BaseAuditableEntityConfiguration<THabit>, IEntityTypeConfiguration<THabit>
        where THabit : BaseAuditableEntity, IHabit<TEntry>
        where TEntry : BaseAuditableEntity, IHabitEntry<THabit>
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