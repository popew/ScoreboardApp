using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public class EffortHabitEntryConfiguration : IEntityTypeConfiguration<EffortHabitEntry>
    {
        public void Configure(EntityTypeBuilder<EffortHabitEntry> builder)
        {
            builder.Property(e => e.Habit)
                .IsRequired();

            builder.Property(e => e.EntryDate)
                .IsRequired();

            builder.Property(e => e.Effort)
                .IsRequired();
        }
    }

    public class CompletionHabitEntryConfiguration : IEntityTypeConfiguration<CompletionHabitEntry>
    {
        public void Configure(EntityTypeBuilder<CompletionHabitEntry> builder)
        {
            builder.Property(e => e.Habit)
                .IsRequired();

            builder.Property(e => e.EntryDate)
                .IsRequired();
        }
    }
}
