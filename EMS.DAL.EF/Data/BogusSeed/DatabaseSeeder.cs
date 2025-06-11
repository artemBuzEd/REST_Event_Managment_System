using Bogus;
using EMS.DAL.EF.Data.BogusSeed.Fakers;
using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Data.BogusSeed;

public class DatabaseSeeder
{
    private EMSManagmentDbContext _context;

    public DatabaseSeeder(EMSManagmentDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Attendees.Any())
        {
            return;
        }

        var attendeesFaker = new AttendeeFaker();
        var attendees = attendeesFaker.Generate(200);
        _context.Attendees.AddRange(attendees);

        var organizerFaker = new OrganizerFaker();
        var organizers = organizerFaker.Generate(20);
        _context.Organizers.AddRange(organizers);

        var venueFaker = new VenueFaker();
        var venues = venueFaker.Generate(20);
        _context.Venues.AddRange(venues);

        var eventCategoryFaker = new EventCategoryFaker();
        var eventCategories = eventCategoryFaker.Generate(20);
        _context.EventCategories.AddRange(eventCategories);

        _context.SaveChanges();

        string[] eventTypeHelper = { "conference", "meeting", "course", "seminar", "expedition" };

        var eventFaker = new Faker<Event>()
            .RuleFor(e => e.Name, f => f.PickRandom(eventTypeHelper))
            .RuleFor(e => e.Description, f => f.Lorem.Sentence())
            .RuleFor(e => e.StartTime, f => f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(2)))
            .RuleFor(e => e.EndTime, (f, e) => { return f.Date.Between(e.StartTime, e.StartTime.AddHours(8)); })
            .RuleFor(e => e.EventCategoryId, f => f.PickRandom(eventCategories).Id)
            .RuleFor(e => e.OrganizerId, f => f.PickRandom(organizers).Id)
            .RuleFor(e => e.VenueId, f => f.PickRandom(venues).Id);

        var events = eventFaker.Generate(20);
        _context.Events.AddRange(events);
        _context.SaveChanges();

        int attendeeCount = 0;
        string[] _status = { "Confirmed", "Cancelled", "Withdrawn" };
        var registrationFaker = new Faker<Registration>()
            .RuleFor(r => r.Status, f => f.PickRandom(_status))
            .RuleFor(r => r.RegistrationDate,
                f => f.Date.Between(DateTime.Now.AddMonths(-5), DateTime.Now.AddMonths(1)))
            .RuleFor(r => r.AttendeeId, f => attendees[attendeeCount++ % attendees.Count].Id)
            .RuleFor(r => r.EventId, f => f.PickRandom(events).Id);
        var registration = registrationFaker.Generate(150);
        _context.Registrations.AddRange(registration);

        _context.SaveChanges();
    }
}