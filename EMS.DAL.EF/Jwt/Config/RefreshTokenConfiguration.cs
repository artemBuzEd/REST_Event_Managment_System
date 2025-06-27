using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.DAL.EF.Jwt.Config;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.UserId).IsRequired();
        
        builder.Property(x => x.Token).IsRequired().HasMaxLength(500);
        
        builder.Property(x => x.Expires).IsRequired();
        
        builder.Property(x => x.Created).IsRequired();
        
        builder.Property(x => x.CreatedByIp).IsRequired().HasMaxLength(50);
        
        builder.Property(x => x.RevokedByIp).HasMaxLength(50);
        
        builder.Property(x => x.ReplacedByToken).HasMaxLength(500);

        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Ignore(x => x.IsExpired);
        builder.Ignore(x => x.IsActive);
    }
}