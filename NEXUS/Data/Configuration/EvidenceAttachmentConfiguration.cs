using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class EvidenceAttachmentConfiguration : IEntityTypeConfiguration<EvidenceAttachment>
{
    public void Configure(EntityTypeBuilder<EvidenceAttachment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Case)
               .WithMany(x => x.Attachments)
               .HasForeignKey(x => x.CaseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Suspect)
               .WithMany() // No collection on Suspect for direct attachments? Let's check Suspect.cs again but I didn't see one.
               .HasForeignKey(x => x.SuspectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
