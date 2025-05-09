namespace EMS.DAL.EF.Entities;

public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int? Capacity { get; set; }
    
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}