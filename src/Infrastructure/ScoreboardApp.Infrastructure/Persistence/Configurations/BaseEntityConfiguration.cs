using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(entity => entity.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
