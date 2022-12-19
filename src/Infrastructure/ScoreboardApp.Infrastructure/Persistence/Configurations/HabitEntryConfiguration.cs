using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Commons;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Interfaces;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public abstract class HabitEntryConfiguration<TEntry, THabit> : BaseAuditableEntityConfiguration<TEntry>, IEntityTypeConfiguration<TEntry>
        where TEntry : BaseAuditableEntity, IHabitEntry<THabit>
        where THabit : BaseAuditableEntity, IHabit<TEntry>
    {
        public override void Configure(EntityTypeBuilder<TEntry> builder)
        {
            base.Configure(builder);

            builder.ToTable(tb => tb.IsTemporal());

            builder.Property(e => e.UserId)
                .HasMaxLength(450);
        }
    }

    public class EffortHabitEntryConfiguration : HabitEntryConfiguration<EffortHabitEntry, EffortHabit>
    {
        public override void Configure(EntityTypeBuilder<EffortHabitEntry> builder)
        {
            base.Configure(builder);
        }
    }

    public class CompletionHabitEntryConfiguration : HabitEntryConfiguration<CompletionHabitEntry, CompletionHabit>
    {
        public override void Configure(EntityTypeBuilder<CompletionHabitEntry> builder)
        {
            base.Configure(builder);
        }
    }
}