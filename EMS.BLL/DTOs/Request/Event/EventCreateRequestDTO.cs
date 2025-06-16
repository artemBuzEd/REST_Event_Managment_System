using System.Runtime.InteropServices.JavaScript;

namespace EMS.BLL.DTOs.Request;

public class EventCreateRequestDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public int EventCategoryId { get; set; }
    public int OrganizerId { get; set; }
    public int VenueId { get; set; }
}