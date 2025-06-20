using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using EMS.DAL.EF.Helpers;
using EMS.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EMS.DAL.EF.Repositories;

public class EMSEventRepository : EMSGenericRepository<Event>, IEMSEventRepository
{
    public EMSEventRepository(EMSManagmentDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<IEnumerable<Event?>> GetByNameAsync(string name)
    {
        return await _context.Events.AsNoTracking()
            .Where(e => e.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }

    public async Task<IEnumerable<Event?>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Events
            .AsNoTracking()
            .Where(e => e.StartTime >= startDate && e.StartTime <= endDate)
            .ToListAsync();
    }
    
    public async Task<Event?> GetByIdWithVenueAndEventCategoryAndOrganizerAsync(int id)
    {
        return await _context.Events.Include(e => e.Venue).Include(e => e.EventCategory).Include(e => e.Organizer).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event?>> GetAllByOrganizerIdAsync(int id)
    {
        return await _context.Events.AsNoTracking().Where(e => e.OrganizerId == id).ToListAsync();
    }

    public async Task<PagedList<Event>> GetAllPaginatedAsync(EventParameters parameters, ISortHelper<Event> sortHelper)
    {
        var query = table.AsQueryable();
        
        if(!string.IsNullOrEmpty(parameters.Name))
            query = query.Where(e => e.Name.ToLower().Contains(parameters.Name.ToLower()));
        
        if(parameters.StartDate != null)
            query = query.Where(e => e.StartTime >= parameters.StartDate);
        
        if(parameters.EndDate != null)
            query = query.Where(e => e.EndTime <= parameters.EndDate);
        
        query = sortHelper.UseSort(query, parameters.OrderBy);
        
        return await PagedList<Event>.ToPagedListAsync(query.AsNoTracking(), parameters.PageNumber, parameters.PageSize);
    }
 }