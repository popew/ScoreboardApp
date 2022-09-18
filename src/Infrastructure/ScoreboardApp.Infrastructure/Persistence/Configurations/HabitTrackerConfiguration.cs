using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    internal class HabitTrackerConfiguration : IEntityTypeConfiguration<HabitTracker>
    {
        public void Configure(EntityTypeBuilder<HabitTracker> builder)
        {
            builder.Property(ht => ht.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
