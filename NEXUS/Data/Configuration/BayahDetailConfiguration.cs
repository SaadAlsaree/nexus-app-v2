using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class BayahDetailConfiguration : IEntityTypeConfiguration<BayahDetail>
{
    public void Configure(EntityTypeBuilder<BayahDetail> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Suspect)
               .WithMany(x => x.BayahDetails)
               .HasForeignKey(x => x.SuspectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
