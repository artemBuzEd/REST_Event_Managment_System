using System.Collections;
using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSAttendeeRepository : EMSGenericRepository<Attendee>, IEMSAttendeeRepository
{
    public EMSAttendeeRepository(EMSManagmentDbContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Attendee?>> GetByFirstNameAsync(string firstName)
    {
        return await _context.Attendees.AsNoTracking()
            .Where(a => a.FirstName == firstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendee?>> GetByLastNameAsync(string lastName)
    {
        return await _context.Attendees.AsNoTracking()
            .Where(a => a.LastName == lastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendee?>> GetByEmailAsync(string email)
    {
        return await _context.Attendees.AsNoTracking()
            .Where(a => a.Email == email)
            .ToListAsync();
    }

    public async Task<Attendee?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Attendees.AsNoTracking()
            .FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumber);
    }
}