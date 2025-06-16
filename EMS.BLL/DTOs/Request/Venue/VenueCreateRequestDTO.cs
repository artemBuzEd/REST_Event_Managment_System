namespace EMS.BLL.DTOs.Request;

public class VenueCreateRequestDTO
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public int? Capacity { get; set; }
}