using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public class HabitTrackerConfiguration : BaseAuditableEntityConfiguration<HabitTracker>, IEntityTypeConfiguration<HabitTracker>
    {
        public override void Configure(EntityTypeBuilder<HabitTracker> builder)
        {
            builder.ToTable(tb => tb.IsTemporal());

            builder.Property(ht => ht.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(ht => ht.UserId)
                .HasMaxLength(450)
                .IsRequired();
        }
    }
}