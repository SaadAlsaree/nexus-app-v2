using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Suspect)
               .WithMany(x => x.Operations)
               .HasForeignKey(x => x.SuspectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
