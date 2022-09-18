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
    internal class HabitConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {
            builder.HasDiscriminator(h => h.HabitType)
                .HasValue<CompletionHabit>(HabitType.Completion)
                .HasValue<EffortBasedHabit>(HabitType.Effort);
        }
    }
}
