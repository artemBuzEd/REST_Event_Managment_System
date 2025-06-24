namespace EMS.BLL.DTOs.Responce;

public class OrganizerFullResponseDTO
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
}