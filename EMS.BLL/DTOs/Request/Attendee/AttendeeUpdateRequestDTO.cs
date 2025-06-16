namespace EMS.BLL.DTOs.Request.Attendee;

public class AttendeeUpdateRequestDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}