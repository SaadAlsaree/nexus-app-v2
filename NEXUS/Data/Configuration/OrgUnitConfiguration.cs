using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class OrgUnitConfiguration : IEntityTypeConfiguration<OrgUnit>
{
    public void Configure(EntityTypeBuilder<OrgUnit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ParentUnit)
               .WithMany(x => x.SubUnits)
               .HasForeignKey(x => x.ParentUnitId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
