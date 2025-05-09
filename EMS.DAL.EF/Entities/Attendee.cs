namespace EMS.DAL.EF.Entities;

public class Attendee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}