using EMS.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.DAL.EF.Data.Config;

public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.ToTable("Registrations");
        builder.HasKey(e => e.Id);

        builder.Property(r => r.RegistrationDate).IsRequired()
            .HasDefaultValueSql("CURDATE()");
        
        builder.Property(r => r.Status).IsRequired()
            .HasMaxLength(50);
        
        builder.HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.Attendee)
            .WithMany(a => a.Registrations)
            .HasForeignKey(r => r.AttendeeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(r => new {r.EventId, r.AttendeeId}).IsUnique();
    }
}