using EMS.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EMS.DAL.EF.Data.Config;

public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
{
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
        builder.ToTable("EventCategories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(c => c.Name);
        
        builder.Property(c => c.Description).HasMaxLength(200);

    }
}