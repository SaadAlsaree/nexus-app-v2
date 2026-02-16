using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Suspect)
               .WithMany(x => x.OrganizationalAssignments)
               .HasForeignKey(x => x.SuspectId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OrgUnit)
               .WithMany(x => x.Assignments)
               .HasForeignKey(x => x.OrgUnitId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DirectCommander)
               .WithMany()
               .HasForeignKey(x => x.DirectCommanderId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
