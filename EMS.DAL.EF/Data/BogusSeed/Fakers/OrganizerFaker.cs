using Bogus;
using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Data.BogusSeed.Fakers;

public sealed class OrganizerFaker : Faker<Organizer>
{
    public OrganizerFaker()
    {
        RuleFor(a => a.Name, f => f.Person.FullName);
        RuleFor(a => a.Email, f => f.Person.Email);
        RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber());
    }
}