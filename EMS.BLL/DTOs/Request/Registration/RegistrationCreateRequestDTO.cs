namespace EMS.BLL.DTOs.Request.Registration;

public class RegistrationCreateRequestDTO
{
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    
    public int AttendeeId { get; set; }
    public int EventId { get; set; }
}