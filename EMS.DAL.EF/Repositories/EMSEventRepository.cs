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
            .Where(e => e.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<IEnumerable<Event?>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Events
            .AsNoTracking()
            .Where(e => e.StartTime >= startDate && e.StartTime <= endDate)
            .ToListAsync();
    }
 }