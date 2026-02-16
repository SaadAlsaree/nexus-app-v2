using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CaseFileNumber)
               .IsRequired();

        builder.HasIndex(x => x.CaseFileNumber);
        builder.HasIndex(x => new { x.IsDeleted, x.CreatedAt });
    }
}
