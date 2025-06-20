using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSOrganizerRepository : EMSGenericRepository<Organizer>, IEMSOrganizerRepository
{
    public EMSOrganizerRepository(EMSManagmentDbContext dbContext) : base(dbContext)
    {
        
    }
    public async Task<IEnumerable<Organizer?>> GetByFullNameAsync(string fullName)
    {
        return await _context.Organizers.AsNoTracking()
            .Where(o => o.Name.ToLower().Contains(fullName.ToLower()))
            .ToListAsync();
    }

    public async Task<IEnumerable<Organizer?>> GetByEmailAsync(string email)
    {
        return await _context.Organizers.AsNoTracking()
            .Where(o => o.Email == email)
            .ToListAsync();
    }

    public async Task<Organizer?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Organizers.AsNoTracking()
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
}