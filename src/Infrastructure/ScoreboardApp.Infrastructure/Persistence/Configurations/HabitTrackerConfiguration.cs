using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    internal class HabitTrackerConfiguration : IEntityTypeConfiguration<HabitTracker>
    {
        public void Configure(EntityTypeBuilder<HabitTracker> builder)
        {
            builder.Property(ht => ht.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}