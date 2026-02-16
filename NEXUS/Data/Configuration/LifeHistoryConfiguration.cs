using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class LifeHistoryConfiguration : IEntityTypeConfiguration<LifeHistory>
{
    public void Configure(EntityTypeBuilder<LifeHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Suspect)
               .WithMany(x => x.LifeHistories)
               .HasForeignKey(x => x.SuspectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
