using Bogus;
using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Data.BogusSeed.Fakers;

public sealed class AttendeeFaker : Faker<Attendee>
{
    public AttendeeFaker()
    {
        RuleFor(a => a.FirstName, f => f.Person.FirstName);
        RuleFor(a => a.LastName, f => f.Person.LastName);
        RuleFor(a => a.Email, f => f.Person.Email);
        RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber());
    }
}