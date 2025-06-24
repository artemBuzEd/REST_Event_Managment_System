namespace EMS.BLL.DTOs.Request.Registration;

public class RegistrationCreateRequestDTO
{
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public string AttendeeId { get; set; } = null!;
    public int EventId { get; set; }
}