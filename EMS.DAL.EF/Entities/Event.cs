namespace EMS.DAL.EF.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    
    public int EventCategoryId { get; set; }
    public string OrganizerId { get; set; } = null!;
    public int VenueId { get; set; }
    
    public virtual EventCategory? EventCategory { get; set; }
    public virtual Organizer? Organizer { get; set; }
    public virtual Venue? Venue { get; set; }
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

}