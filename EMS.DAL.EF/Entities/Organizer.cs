namespace EMS.DAL.EF.Entities;

public class Organizer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}