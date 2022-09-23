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
            builder.Property(ht => ht.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}