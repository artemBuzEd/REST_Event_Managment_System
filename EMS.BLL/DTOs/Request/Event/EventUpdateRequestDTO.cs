namespace EMS.BLL.DTOs.Request;

public class EventUpdateRequestDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public int VenueId { get; set; }
    public int EventCategoryId { get; set; }
}