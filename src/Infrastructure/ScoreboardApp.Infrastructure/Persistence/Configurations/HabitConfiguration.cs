using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Commons;
using ScoreboardApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public class HabitConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {
            builder.HasDiscriminator<HabitType>("HabitType")
                .HasValue<CompletionHabit>(HabitType.Completion)
                .HasValue<EffortBasedHabit>(HabitType.Effort);

            builder.Property(h => h.HabitTracker)
                .IsRequired();

            builder.Property(h => h.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
