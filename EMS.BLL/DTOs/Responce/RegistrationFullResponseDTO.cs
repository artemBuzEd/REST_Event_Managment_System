namespace EMS.BLL.DTOs.Responce;

public class RegistrationFullResponseDTO
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public string AttendeeId { get; set; }
    public int EventId { get; set; }
}