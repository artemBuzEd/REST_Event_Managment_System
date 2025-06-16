namespace EMS.BLL.DTOs.Request;

public class EventUpdateRequestDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}