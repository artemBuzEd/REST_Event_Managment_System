namespace EMS.DAL.EF.Entities.HelpModels;

public class EventParameters : QueryStringParameters
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}