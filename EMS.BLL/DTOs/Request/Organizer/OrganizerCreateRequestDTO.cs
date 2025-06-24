namespace EMS.BLL.DTOs.Request;

public class OrganizerCreateRequestDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; }
}