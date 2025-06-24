using EMS.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.DAL.EF.Data.Config;

public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        
        builder.Property(a => a.FirstName).IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.LastName).IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Email).IsRequired()
            .HasMaxLength(150);
        
        builder.HasIndex(a => a.Email).IsUnique();
        
    }
}