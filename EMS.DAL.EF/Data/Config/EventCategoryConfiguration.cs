using EMS.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EMS.DAL.EF.Data.Config;

public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
{
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
        builder.ToTable("EventCategory");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(c => c.Name).IsUnique();
        
        builder.Property(c => c.Description).HasMaxLength(200);

    }
}