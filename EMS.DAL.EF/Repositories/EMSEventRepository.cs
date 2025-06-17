using EMS.DAL.EF.Data;
using EMS.DAL.EF.Entities;
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
 }