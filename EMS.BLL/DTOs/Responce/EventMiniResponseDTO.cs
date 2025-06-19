namespace EMS.BLL.DTOs.Responce;

public class EventMiniResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
}