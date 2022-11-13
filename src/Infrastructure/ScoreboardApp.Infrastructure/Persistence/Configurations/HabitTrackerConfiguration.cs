using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public class HabitTrackerConfiguration : BaseAuditableEntityConfiguration<HabitTracker>, IEntityTypeConfiguration<HabitTracker>
    {
        public override void Configure(EntityTypeBuilder<HabitTracker> builder)
        {
            builder.HasMany(ht => ht.EffortHabits)
                .WithOne(h => h.HabitTracker)
                .HasForeignKey(h => h.HabitTrackerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ht => ht.CompletionHabits)
                .WithOne(h => h.HabitTracker)
                .HasForeignKey(h => h.HabitTrackerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ht => ht.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}