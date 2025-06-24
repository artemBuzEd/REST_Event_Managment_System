using EMS.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.DAL.EF.Data.Config;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name).IsRequired()
            .HasMaxLength(200);
        
        builder.Property(e => e.Description).HasMaxLength(500);
        
        builder.Property(e => e.StartTime).IsRequired()
            .HasColumnType("datetime");
        
        builder.Property(e => e.EndTime).IsRequired()
            .HasColumnType("datetime");
        
        //builder.Property(e => e.CreatedOn).IsRequired()
        //    .HasDefaultValueSql("CURDATE()");
        
        // Organizer - Event RELATIONSHIP
        builder.HasOne(e => e.Organizer)
            .WithMany(o => o.Events)
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);
        // Venue - Event RELATIONSHIP
        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
        // EventCategory - Event RELATIONSHIP
        builder.HasOne(e => e.EventCategory)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.EventCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}