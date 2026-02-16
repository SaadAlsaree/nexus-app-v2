using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class EntityWatchlistConfiguration : IEntityTypeConfiguration<EntityWatchlist>
{
    public void Configure(EntityTypeBuilder<EntityWatchlist> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
