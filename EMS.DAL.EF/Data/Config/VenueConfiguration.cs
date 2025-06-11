using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EMS.DAL.EF.Entities;
namespace EMS.DAL.EF.Data.Config;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Name).IsRequired()
            .HasMaxLength(150);
        
        builder.Property(v => v.Address).IsRequired()
            .HasMaxLength(200);
        
        builder.Property(v => v.City).IsRequired()
            .HasMaxLength(100);
        
        builder.Property(v => v.Country).IsRequired()
            .HasMaxLength(100);
    }
}