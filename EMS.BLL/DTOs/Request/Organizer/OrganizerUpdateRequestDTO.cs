namespace EMS.BLL.DTOs.Request;

public class OrganizerUpdateRequestDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}