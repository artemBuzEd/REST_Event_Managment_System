namespace EMS.DAL.EF.Entities.HelpModels;

public class AttendeeParameters : QueryStringParameters
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}