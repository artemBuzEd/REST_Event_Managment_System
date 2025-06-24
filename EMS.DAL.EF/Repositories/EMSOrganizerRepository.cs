using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSOrganizerRepository : EMSGenericRepository<Organizer>, IEMSOrganizerRepository
{
    private readonly UserManager<User> _userManager;
    public EMSOrganizerRepository(EMSDbContext dbContext, UserManager<User> userManager) : base(dbContext)
    {
        _userManager = userManager;
    }

    public new IQueryable<Organizer> GetAllAsync()
    {
        return _context.Users.OfType<Organizer>().AsNoTracking();
    }

    public async Task<IEnumerable<Organizer?>> GetByFullNameAsync(string fullName)
    {
        return await GetAllAsync()
            .Where(o => o.Name.ToLower().Contains(fullName.ToLower()))
            .ToListAsync();
    }

    public async Task<IEnumerable<Organizer?>> GetByEmailAsync(string email)
    {
        return await GetAllAsync()
            .Where(o => o.Email == email)
            .ToListAsync();
    }

    public async Task<Organizer?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await GetAllAsync()
            .FirstOrDefaultAsync(o => o.PhoneNumber == phoneNumber);
    }

    public async Task<PagedList<Organizer>> GetAllPaginatedAsync(OrganizerParameters parameters, ISortHelper<Organizer> sortHelper)
    {
        var query = table.AsQueryable();
        
        if(!string.IsNullOrWhiteSpace(parameters.Name))
            query = query.Where(e => e.Name.ToLower().Contains(parameters.Name.ToLower()));
        
        if(!string.IsNullOrWhiteSpace(parameters.Email))
            query = query.Where(e => e.Email.ToLower().Contains(parameters.Email.ToLower()));
        
        if(!string.IsNullOrWhiteSpace(parameters.PhoneNumber))
            query = query.Where(e => e.PhoneNumber.Contains(parameters.PhoneNumber));
        
        query = sortHelper.UseSort(query, parameters.OrderBy);
        
        return await PagedList<Organizer>.ToPagedListAsync(query.AsNoTracking(), parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Organizer?> GetByIdAsync(string id)
    {
        return await _context.Users.OfType<Organizer>().FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<(IdentityResult, Organizer)> CreateAsync(Organizer organizer, string password)
    {
        var result = await _userManager.CreateAsync(organizer, password);
        if(result.Succeeded)
            await _userManager.AddToRoleAsync(organizer, "Organizer");
        return (result, organizer);
    }

    public new async Task<IdentityResult> UpdateAsync(Organizer organizer)
    {
        return await _userManager.UpdateAsync(organizer);
    }

    public new async Task<IdentityResult> DeleteAsync(Organizer organizer)
    {
        return await _userManager.DeleteAsync(organizer);
    }
}