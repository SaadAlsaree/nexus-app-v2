using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class InterrogationSessionConfiguration : IEntityTypeConfiguration<InterrogationSession>
{
    public void Configure(EntityTypeBuilder<InterrogationSession> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InterrogatorName)
               .HasMaxLength(100);

        builder.Property(x => x.Content)
               .IsRequired();

        builder.HasOne(s => s.Suspect)
               .WithMany()
               .HasForeignKey(s => s.SuspectId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Case)
               .WithMany()
               .HasForeignKey(s => s.CaseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
