namespace EMS.DAL.EF.Entities;

public class Registration
{
    public int Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = "Confirmed";
    
    public int AttendeeId { get; set; }
    public int EventId { get; set; }
    
    public virtual Event? Event { get; set; }
    public virtual Attendee? Attendee { get; set; }
}