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
        }
    }
}