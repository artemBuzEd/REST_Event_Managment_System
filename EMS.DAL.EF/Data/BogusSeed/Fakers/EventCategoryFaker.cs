using Bogus;
using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Data.BogusSeed.Fakers;

public sealed class EventCategoryFaker : Faker<EventCategory>
{
    private string[] _categoryHelper =
    {
        "Music", "Sports", "Technology", "Business", "Health & Wellness",
        "Education", "Art & Culture", "Food & Drink", "Travel", "Fashion"
    };
    
    public EventCategoryFaker()
    {
        RuleFor(a => a.Name, f => $"{f.PickRandom(_categoryHelper)}");
        RuleFor(a => a.Description, f => f.Random.Word());
    }
}