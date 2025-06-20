namespace EMS.DAL.EF.Entities.HelpModels;

public class OrganizerParameters : QueryStringParameters
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}