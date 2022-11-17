using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScoreboardApp.Domain.Commons;

namespace ScoreboardApp.Infrastructure.Persistence.Configurations
{
    public class BaseAuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseAuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            builder.Property(entity => entity.LastModifiedBy)
                .HasMaxLength(200);

            builder.Property(entity => entity.CreatedBy)
                .HasMaxLength(200);
        }
    }
}