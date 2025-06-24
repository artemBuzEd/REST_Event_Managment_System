namespace EMS.DAL.EF.Entities;

public class Organizer : User
{
    public string Name { get; set; }
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}