namespace EMS.DAL.EF.Entities;

public class Attendee : User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}