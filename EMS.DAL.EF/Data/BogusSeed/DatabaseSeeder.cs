using Bogus;
using EMS.DAL.EF.Data.BogusSeed.Fakers;
using EMS.DAL.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.DAL.EF.Data.BogusSeed;

public static class DatabaseSeeder
{
     public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<EMSDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        await context.Database.EnsureCreatedAsync();

        if (context.Users.Any())
            return;
        
        await roleManager.CreateAsync(new IdentityRole("Attendee"));
        await roleManager.CreateAsync(new IdentityRole("Organizer"));

        var attendees = new List<Attendee>();
        var attendeeFaker = new AttendeeFaker();
        
        for (int i = 0; i < 200; i++)
        {
            var attendee = attendeeFaker.Generate();
            var result = await userManager.CreateAsync(attendee, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(attendee, "Attendee");
                attendees.Add(attendee);
            }
        }
        
        var organizers = new List<Organizer>();
        var organizerFaker = new OrganizerFaker();
        
        for (int i = 0; i < 20; i++)
        {
            var organizer = organizerFaker.Generate();
            var result = await userManager.CreateAsync(organizer, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(organizer, "Organizer");
                organizers.Add(organizer);
            }
        }

        var venueFaker = new VenueFaker();
        var venues = venueFaker.Generate(20);
        context.Venues.AddRange(venues);

        var eventCategoryFaker = new EventCategoryFaker();
        var eventCategories = eventCategoryFaker.Generate(10);
        context.EventCategories.AddRange(eventCategories);

        await context.SaveChangesAsync();

        string[] eventTypeHelper = { "conference", "meeting", "course", "seminar", "expedition" };

        var eventFaker = new Faker<Event>()
            .RuleFor(e => e.Name, f => f.PickRandom(eventTypeHelper))
            .RuleFor(e => e.Description, f => f.Lorem.Sentence())
            .RuleFor(e => e.StartTime, f => f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(2)))
            .RuleFor(e => e.EndTime, (f, e) => f.Date.Between(e.StartTime, e.StartTime.AddHours(8)))
            .RuleFor(e => e.EventCategoryId, f => f.PickRandom(eventCategories).Id)
            .RuleFor(e => e.OrganizerId, f => f.PickRandom(organizers).Id)
            .RuleFor(e => e.VenueId, f => f.PickRandom(venues).Id);

        var events = eventFaker.Generate(20);
        context.Events.AddRange(events);
        await context.SaveChangesAsync();

        int attendeeCount = 0;
        string[] _status = { "Confirmed", "Cancelled", "Withdrawn" };
        var registrationFaker = new Faker<Registration>()
            .RuleFor(r => r.Status, f => f.PickRandom(_status))
            .RuleFor(r => r.RegistrationDate,
                f => f.Date.Between(DateTime.Now.AddMonths(-5), DateTime.Now.AddMonths(1)))
            .RuleFor(r => r.AttendeeId, f => attendees[attendeeCount++ % attendees.Count].Id)
            .RuleFor(r => r.EventId, f => f.PickRandom(events).Id);
        
        var registration = registrationFaker.Generate(150);
        context.Registrations.AddRange(registration);

        await context.SaveChangesAsync();
    }
}