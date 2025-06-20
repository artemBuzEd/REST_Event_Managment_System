namespace EMS.DAL.EF.Entities.HelpModels;

public class VenueParameters : QueryStringParameters
{
    public string? Name { get; set; }
    public int? MinCapacity { get; set; }
    public int? MaxCapacity { get; set; }
}