using System.Collections;
using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
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
            .Where(a => a.FirstName.ToLower() == firstName.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendee?>> GetByLastNameAsync(string lastName)
    {
        return await _context.Attendees.AsNoTracking()
            .Where(a => a.LastName.ToLower() == lastName.ToLower())
            .ToListAsync();
    }

    public async Task<Attendee?> GetByEmailAsync(string email)
    {
        return await _context.Attendees.AsNoTracking()
            .Where(a => a.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<Attendee?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Attendees.AsNoTracking()
            .FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumber);
    }

    public async Task<PagedList<Attendee>> GetAllPaginatedAsync(AttendeeParameters parameters,
        ISortHelper<Attendee> sortHelper)
    {
        var query = table.AsQueryable();
        
        if(!string.IsNullOrWhiteSpace(parameters.FirstName))
            query = query.Where(a => a.FirstName.ToLower().Contains(parameters.FirstName.ToLower()));
        
        if(!string.IsNullOrWhiteSpace(parameters.LastName))
            query = query.Where(a => a.LastName.ToLower().Contains(parameters.LastName.ToLower()));
        
        if(!string.IsNullOrWhiteSpace(parameters.Email))
            query = query.Where(a => a.Email.ToLower().Contains(parameters.Email.ToLower()));
        
        if(!string.IsNullOrWhiteSpace(parameters.PhoneNumber))
            query = query.Where(a => a.PhoneNumber.ToLower().Contains(parameters.PhoneNumber.ToLower()));
        
        query = sortHelper.UseSort(query, parameters.OrderBy);
        
        return await PagedList<Attendee>.ToPagedListAsync(query.AsNoTracking(), parameters.PageNumber, parameters.PageSize);
    }
}