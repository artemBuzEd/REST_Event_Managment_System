using System.Collections;
using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSAttendeeRepository : EMSGenericRepository<Attendee>, IEMSAttendeeRepository
{
    private readonly UserManager<User> _userManager;
    public EMSAttendeeRepository(EMSDbContext context, UserManager<User> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public new IQueryable<Attendee> GetAllAsync()
    {
        return _context.Users.OfType<Attendee>().AsNoTracking();
    }
    public async Task<IEnumerable<Attendee?>> GetByFirstNameAsync(string firstName)
    {
        return await GetAllAsync()
            .Where(a => a.FirstName.ToLower() == firstName.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendee?>> GetByLastNameAsync(string lastName)
    {
        return await GetAllAsync()
            .Where(a => a.LastName.ToLower() == lastName.ToLower())
            .ToListAsync();
    }

    public async Task<Attendee?> GetByEmailAsync(string email)
    {
        return await GetAllAsync()
            .Where(a => a.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<Attendee?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await GetAllAsync()
            .FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumber);
    }

    public async Task<PagedList<Attendee>> GetAllPaginatedAsync(AttendeeParameters parameters,
        ISortHelper<Attendee> sortHelper)
    {
        var query = GetAllAsync();
        
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

    public async Task<Attendee?> GetByIdAsync(string id)
    {
        return await GetAllAsync().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<(IdentityResult, Attendee)> CreateAsync(Attendee attendee, string password)
    {
        var result = await _userManager.CreateAsync(attendee, password);
        if(result.Succeeded)
            await _userManager.AddToRoleAsync(attendee, "Attendee");
        
        return (result, attendee);
    }

    public new async Task<IdentityResult> UpdateAsync(Attendee attendee)
    {
        return await _userManager.UpdateAsync(attendee);
    }
    
    public new async Task<IdentityResult> DeleteAsync(Attendee attendee)
    {
        return await _userManager.DeleteAsync(attendee);
    }
}