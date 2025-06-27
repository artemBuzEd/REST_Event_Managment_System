using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Data
{
    public partial class EMSDbContext : IdentityDbContext<User>
    {
        public EMSDbContext(DbContextOptions<EMSDbContext> options) : base(options)
        {
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=EMS;User=root;Password=", ServerVersion.AutoDetect("server=localhost;database=EMS;User=root;Password"));
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
        
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<EventCategory> EventCategories { get; set; } = null!;
        public virtual DbSet<Registration> Registrations { get; set; } = null!;
        public virtual DbSet<Venue> Venues { get; set; } = null!;
        
        public virtual DbSet<Attendee> Attendees { get; set; } = null!;
        
        public virtual DbSet<Organizer> Organizers { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    }
}