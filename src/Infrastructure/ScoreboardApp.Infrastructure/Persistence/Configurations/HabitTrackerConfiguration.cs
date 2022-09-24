using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;
using ScoreboardApp.Domain.Entities.Commons;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public class HabitTrackerConfiguration : IEntityTypeConfiguration<HabitTracker>
    {
        public void Configure(EntityTypeBuilder<HabitTracker> builder)
        {
            builder.HasMany(ht => ht.EffortHabits)
                .WithOne(h => h.HabitTracker)
                .HasForeignKey(h => h.HabitTrackerId);

            builder.HasMany(ht => ht.CompletionHabits)
                .WithOne(h => h.HabitTracker)
                .HasForeignKey(h => h.HabitTrackerId);

            builder.Property(ht => ht.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}