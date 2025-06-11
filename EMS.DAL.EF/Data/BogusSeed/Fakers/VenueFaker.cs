using Bogus;
using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Data.BogusSeed.Fakers;

public sealed class VenueFaker : Faker<Venue>
{
    public VenueFaker()
    {
        RuleFor(a => a.Name, f => $"{f.Company.CompanyName()} {f.Commerce.Department()} Hall");
        RuleFor(a => a.Address, f => f.Address.StreetAddress());
        RuleFor(a => a.City, f => f.Address.City());
        RuleFor(a => a.Country, f => f.Address.Country());
        RuleFor(a => a.Capacity, f => f.Random.Number(1, 100));
    }
}