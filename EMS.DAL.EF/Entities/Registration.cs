namespace EMS.DAL.EF.Entities;

public class Registration
{
    public int Id { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Confirmed";

    public string AttendeeId { get; set; } = null!;
    public int EventId { get; set; }
    
    public virtual Event? Event { get; set; }
    public virtual Attendee? Attendee { get; set; }
}