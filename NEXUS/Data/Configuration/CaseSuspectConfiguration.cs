using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class CaseSuspectConfiguration : IEntityTypeConfiguration<CaseSuspect>
{
    public void Configure(EntityTypeBuilder<CaseSuspect> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Case)
               .WithMany(x => x.SuspectsInvolved)
               .HasForeignKey(x => x.CaseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Suspect)
               .WithMany(x => x.CaseInvolvements)
               .HasForeignKey(x => x.SuspectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
