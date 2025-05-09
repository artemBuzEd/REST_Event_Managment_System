namespace EMS.DAL.EF.Entities;

public class EventCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}