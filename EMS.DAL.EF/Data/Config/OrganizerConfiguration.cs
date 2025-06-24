using EMS.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.DAL.EF.Data.Config;

public class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> builder)
    {
        
        builder.Property(o => o.Name).IsRequired()
            .HasMaxLength(150);
        
        builder.Property(o => o.Email).IsRequired()
            .HasMaxLength(150);
        
        builder.HasIndex(o => o.Email).IsUnique();
        
    }
}