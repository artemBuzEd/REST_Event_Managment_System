namespace EMS.BLL.DTOs.Responce;

public class EventFullResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
    
    public EventCategoryMiniResponseDTO Category { get; set; } = new();
    public VenueFullResponseDTO Venue { get; set; } = new();
    public OrganizerFullResponseDTO Organizer { get; set; } = new();
}