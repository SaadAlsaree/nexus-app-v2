using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NEXUS.Data.Entities;

namespace NEXUS.Data.Configuration;

public class SuspectConfiguration : IEntityTypeConfiguration<Suspect>
{
    public void Configure(EntityTypeBuilder<Suspect> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.CurrentStatus)
               .IsRequired();

        builder.Property(x => x.Kunya)
               .HasMaxLength(100);

        builder.Property(x => x.MotherName)
               .HasMaxLength(100);

        builder.HasIndex(x => x.CodeNum);
    }
}
